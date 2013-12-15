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

using System.Runtime.InteropServices;

namespace Beanulator.Common.Processors.SONY
{
    public abstract class SPC700 : Processor
    {
        private Flags flags;
        private Pins pins;
        private Registers registers;

        private void modify(byte value)
        {
            flags.n = value >= 0x80;
            flags.z = value == 0x00;
        }

        #region modes

        private void am_dpg()
        {
            registers.ea = read(registers.pc++);

            if (flags.p)
                registers.eah = 1;
        }
        private void am_dpx()
        {
            am_dpg();

            registers.eal += registers.x;
        }
        private void am_dpy()
        {
            am_dpg();

            registers.eal += registers.y;
        }

        #endregion
        #region codes

        private void op_cmp()
        {
            modify(Alu.Sub(registers.l, registers.r));
            flags.c = Alu.c != 0;
        }
        private void op_dec() { modify(--registers.l); }
        private void op_inc() { modify(++registers.l); }

        #endregion
        #region access patterns

        #endregion

        protected abstract byte read();
        protected abstract void write();

        protected byte read(ushort address)
        {
            pins.ab = address;

            read();

            return pins.db;
        }
        protected void write(ushort address, byte data)
        {
            pins.ab = address;
            pins.db = data;

            write();
        }

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
        }
        public struct Flags
        {
            public bool n, v, p, b, h, i, z, c;

            public void load(byte value)
            {
                n = (value & 0x80) != 0;
                v = (value & 0x40) != 0;
                p = (value & 0x20) != 0;
                b = (value & 0x10) != 0;
                h = (value & 0x08) != 0;
                i = (value & 0x04) != 0;
                z = (value & 0x02) != 0;
                c = (value & 0x01) != 0;
            }
            public byte save()
            {
                return (byte)(
                    (n ? 0x80 : 0) |
                    (v ? 0x40 : 0) |
                    (p ? 0x20 : 0) |
                    (b ? 0x10 : 0) |
                    (h ? 0x08 : 0) |
                    (i ? 0x04 : 0) |
                    (z ? 0x02 : 0) |
                    (c ? 0x01 : 0));
            }
        }
        public struct Pins
        {
            public ushort ab;
            public byte db;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct Registers
        {
            [FieldOffset(0x0)] public byte a;
            [FieldOffset(0x1)] public byte y;
            [FieldOffset(0x2)] public byte x;
            [FieldOffset(0x3)] public byte l;
            [FieldOffset(0x4)] public byte r;

            [FieldOffset(0x6)] public byte pcl;
            [FieldOffset(0x7)] public byte pch;
            [FieldOffset(0x8)] public byte spl;
            [FieldOffset(0x9)] public byte sph;
            [FieldOffset(0xa)] public byte eal;
            [FieldOffset(0xb)] public byte eah;
            [FieldOffset(0xc)] public byte idl;
            [FieldOffset(0xd)] public byte idh;

            [FieldOffset(0x0)] public ushort ya;
            [FieldOffset(0x6)] public ushort pc;
            [FieldOffset(0x8)] public ushort sp;
            [FieldOffset(0xa)] public ushort ea;
            [FieldOffset(0xc)] public ushort id;
        }
    }
}
