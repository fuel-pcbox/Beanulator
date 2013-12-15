// beanulator's code is licensed under the 4 clause BSD license:
//
// Copyright (c) 2013, beannaich
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by beannaich.
// 4. Neither the name of beanulator nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDERS BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Runtime.InteropServices;

namespace Beanulator.Common.Processors.MOS
{
    /// <summary>
    /// The MOS Technology 6502
    /// </summary>
    public abstract class R6502 : Processor
    {
        public Flags flags;
        public Interrupts interrupts;
        public Pins pins;
        public Registers registers;
        public byte code;

        private void branch(bool value)
        {
            registers.t = read(registers.pc);
            registers.pc++;

            if (value)
            {
                read(registers.pc);
                registers.pcl = Alu.Add(registers.pcl, registers.t);

                switch (registers.t >> 7)
                {
                case 0: if (Alu.c == 1) { read(registers.pc); registers.pch++; } break; // positive offset, addition carried
                case 1: if (Alu.c == 0) { read(registers.pc); registers.pch--; } break; // negative offset, addition borrowed
                }
            }
        }
        private void modify(byte value)
        {
            flags.n = value >> 7;
            flags.z = value == 0 ? 1 : 0;
        }

        private void poll_interrupt()
        {
            if (!interrupts.res_latch && pins.res) interrupts.res = true;
            if (!interrupts.nmi_latch && pins.nmi) interrupts.nmi = true;

            interrupts.res_latch = pins.res;
            interrupts.nmi_latch = pins.nmi;
            interrupts.irq = (pins.irq & ~flags.i) != 0;
        }

        #region modes

        #endregion
        #region codes

        #endregion
        #region access patterns

        #endregion

        protected void step()
        {
            code = read(registers.pc);

            if (interrupts.available)
            {
                code = 0;
            }

            // table[code]();
        }
        protected byte read(ushort address)
        {
            pins.address = address;
            pins.read = true;

            tick(1);

            return pins.data;
        }
        protected void write(ushort address, byte data)
        {
            pins.address = address;
            pins.data = data;
            pins.read = false;

            tick(1);
        }

        public void irq(bool assert, int type) { pins.irq = assert ? (pins.irq | type) : (pins.irq & ~type); }
        public void nmi(bool assert) { pins.nmi = assert; }
        public void res(bool assert) { pins.res = assert; }

        public struct Alu
        {
            public static int c;
            public static int v;

            public static byte Add(byte a, byte b, int carry = 0)
            {
                byte temp = (byte)((a + b) + carry);
                byte bits = (byte)(~(a ^ b) & (a ^ temp));

                c = (bits ^ temp ^ a ^ b) >> 7;
                v = (bits) >> 7;

                return temp;
            }
            public static byte Sub(byte a, byte b, int carry = 1)
            {
                b ^= 0xff;
                return Add(a, b, carry);
            }
            public static byte Shl(byte a, int carry = 0)
            {
                c = (a >> 7);
                return (byte)((a << 1) | (carry >> 0));
            }
            public static byte Shr(byte a, int carry = 0)
            {
                c = (a & 1);
                return (byte)((a >> 1) | (carry << 7));
            }
        }
        public struct Flags
        {
            public int n;
            public int v;
            public int d;
            public int i;
            public int z;
            public int c;

            public void load(byte value)
            {
                n = (value & 0x80) >> 7;
                v = (value & 0x40) >> 6;
                d = (value & 0x08) >> 3;
                i = (value & 0x04) >> 2;
                z = (value & 0x02) >> 1;
                c = (value & 0x01) >> 0;
            }
            public byte save(byte value = 0x30)
            {
                return (byte)(
                    (n << 7) |
                    (v << 6) |
                    (d << 3) |
                    (i << 2) |
                    (z << 1) |
                    (c << 0) | value);
            }
        }
        public struct Interrupts
        {
            public bool res, res_latch;
            public bool nmi, nmi_latch;
            public bool irq;
            public bool available;
        }
        public struct Pins
        {
            public bool read;       // read/write flag
            public bool rdy;        // allows processor to execute, used to implement dma schemes or wait states.
            public bool nmi;        // non-maskable interrupt
            public int  irq;        // interrupt request. integer used to accumulate irq lines, simplifying irq logic
            public bool res;        // processor reset
            public ushort address;  // address bus
            public byte data;       // data bus
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct Registers
        {
            [FieldOffset(0x0)] public ushort pc;    // program cursor
            [FieldOffset(0x2)] public ushort sp;    // stack pointer
            [FieldOffset(0x4)] public ushort ea;    // effective address temporary
            [FieldOffset(0x6)] public ushort id;    // indirect address temporary
            [FieldOffset(0x8)] public byte a;       // accumulator
            [FieldOffset(0x9)] public byte x;       // x-index register
            [FieldOffset(0xa)] public byte y;       // y-index register
            [FieldOffset(0xb)] public byte t;       // temporary, data bus latch

            #region 16-bit accessors

            [FieldOffset(0x0)] public byte pcl;
            [FieldOffset(0x1)] public byte pch;
            [FieldOffset(0x2)] public byte spl;
            [FieldOffset(0x3)] public byte sph;
            [FieldOffset(0x4)] public byte eal;
            [FieldOffset(0x5)] public byte eah;
            [FieldOffset(0x6)] public byte idl;
            [FieldOffset(0x7)] public byte idh;

            #endregion
        }
    }
}
