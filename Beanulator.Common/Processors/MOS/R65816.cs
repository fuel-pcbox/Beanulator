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

namespace Beanulator.Common.Processors.MOS
{
    public abstract class R65816 : Processor
    {
        private Pins pins;
        private Registers registers;
        private byte dbr;
        private byte pbr;

        #region modes

        private void am_abs()
        {
            registers.eal = read(registers.pc++);
            registers.eah = read(registers.pc++);
            pins.bank = dbr;
        }
        private void am_abx()
        {
            am_abs();

            if ((registers.ea += registers.x) < registers.x)
            {
                pins.bank++;
            }
        }
        private void am_aby()
        {
            am_abs();

            if ((registers.ea += registers.y) < registers.y)
            {
                pins.bank++;
            }
        }
        private void am_dpg()
        {
            registers.eal = read(registers.pc++);
            registers.eah = registers.dph;
            pins.bank = 0;

            if (registers.dpl != 0)
            {
                idle();
                registers.ea += registers.dpl;
            }
        }
        private void am_dpx()
        {
            am_dpg();

            idle();
            registers.ea += registers.x;
        }
        private void am_dpy()
        {
            am_dpg();

            idle();
            registers.ea += registers.y;
        }

        private void am_abs_ind() { }
        private void am_dpg_ind()
        {
            am_dpg();

            registers.idl = read(registers.ea); registers.ea++;
            registers.idh = read(registers.ea);

            registers.ea = registers.id;
        }
        private void am_dpx_ind()
        {
            am_dpx();

            registers.idl = read(registers.ea); registers.ea++;
            registers.idh = read(registers.ea);

            registers.ea = registers.id;
        }
        private void am_dpy_ind()
        {
            am_dpg();

            registers.idl = read(registers.ea); registers.ea++;
            registers.idh = read(registers.ea);

            registers.ea = registers.id;
            registers.ea += registers.y;
        }

        #endregion
        #region codes

        #endregion
        #region access patterns

        #endregion

        protected abstract void idle();
        protected abstract void read();
        protected abstract void write();

        public byte read(ushort address)
        {
            pins.address = address;

            read();

            return pins.data;
        }
        public void write(ushort address, byte data)
        {
            pins.address = address;
            pins.data = data;

            write();
        }

        public struct Pins
        {
            public ushort address;
            public byte bank;
            public byte data;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct Registers
        {
            [FieldOffset(0x0)] public ushort a;     // accumulator
            [FieldOffset(0x2)] public ushort x;     // x-index register
            [FieldOffset(0x4)] public ushort y;     // y-index register
            [FieldOffset(0x6)] public ushort dp;    // direct page
            [FieldOffset(0x8)] public ushort pc;    // program cursor
            [FieldOffset(0xa)] public ushort sp;    // stack pointer
            [FieldOffset(0xc)] public ushort ea;    // effective address temporary register
            [FieldOffset(0xe)] public ushort id;    // indirect address temporary register

            #region 16-bit accessors

            [FieldOffset(0x0)] public byte al;
            [FieldOffset(0x1)] public byte ah;
            [FieldOffset(0x2)] public byte xl;
            [FieldOffset(0x3)] public byte xh;
            [FieldOffset(0x4)] public byte yl;
            [FieldOffset(0x5)] public byte yh;
            [FieldOffset(0x6)] public byte dpl;
            [FieldOffset(0x7)] public byte dph;
            [FieldOffset(0x8)] public byte pcl;
            [FieldOffset(0x9)] public byte pch;
            [FieldOffset(0xa)] public byte spl;
            [FieldOffset(0xb)] public byte sph;
            [FieldOffset(0xc)] public byte eal;
            [FieldOffset(0xd)] public byte eah;
            [FieldOffset(0xe)] public byte idl;
            [FieldOffset(0xf)] public byte idh;

            #endregion
        }
    }
}
