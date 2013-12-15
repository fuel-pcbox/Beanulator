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

namespace Beanulator.Common.Processors.ARM
{
    public class ARM7 : Processor
    {
        private Action[] code;

        private Mode abt = new Mode(2);
        private Mode fiq = new Mode(7);
        private Mode irq = new Mode(2);
        private Mode svc = new Mode(2);
        private Mode und = new Mode(2);
        private Mode usr = new Mode(7);

        #region active state

        private Flags cpsr;
        private Flags spsr;
        private Register lr;
        private Register pc;
        private Register[] registers;

        #endregion

        public ARM7()
        {
            cpsr = new Flags();
            spsr = null;
            registers = new Register[16];
            registers.Initialize<Register>();

            code = new Action[encode(~0u) + 1u];

            #region initialize instruction table



            #endregion

            exception_res();
        }

        private void map(string pattern, Action code)
        {
            uint bitx = encode(Utility.Pattern(pattern, 0, 0, 1)); // pick out '-' bits
            uint bit0 = encode(Utility.Pattern(pattern, 0, 1, 0)); // pick out '0' bits
            uint bit1 = encode(Utility.Pattern(pattern, 1, 0, 0)); // pick out '1' bits

            for (uint bits = bit1; bits <= (bitx | bit1); bits++) // binary search for instruction matches
            {
                if ((bits & (bit0 | bit1)) == bit1)
                {
                    this.code[bits] = code;
                }
            }
        }

        private void change(uint mode)
        {
            var bank = (mode == Mode.FIQ) ? fiq : usr;

            registers[ 8] = bank.registers[2];
            registers[ 9] = bank.registers[3];
            registers[10] = bank.registers[4];
            registers[11] = bank.registers[5];
            registers[12] = bank.registers[6];

            switch (mode)
            {
            case Mode.USR: registers[13] = usr.registers[0]; registers[14] = usr.registers[1]; spsr = null; break;
            case Mode.FIQ: registers[13] = fiq.registers[0]; registers[14] = fiq.registers[1]; spsr = fiq.spsr; break;
            case Mode.IRQ: registers[13] = irq.registers[0]; registers[14] = irq.registers[1]; spsr = irq.spsr; break;
            case Mode.SVC: registers[13] = svc.registers[0]; registers[14] = svc.registers[1]; spsr = svc.spsr; break;
            case Mode.ABT: registers[13] = abt.registers[0]; registers[14] = abt.registers[1]; spsr = abt.spsr; break;
            case Mode.UND: registers[13] = und.registers[0]; registers[14] = und.registers[1]; spsr = und.spsr; break;
            case Mode.SYS: registers[13] = usr.registers[0]; registers[14] = usr.registers[1]; spsr = null; break;
            }

            lr = registers[14];
            pc = registers[15];
        }
        private uint encode(uint code)
        {
            return ((code >> 16) & 0xff0) | ((code >> 4) & 0x00f);
        }
        private void vector(uint mode, uint address, uint i, uint f)
        {
            change(mode);

            spsr.copy(cpsr);

            cpsr.i = i;
            cpsr.f = f;
            cpsr.t = 0;

            lr.set(pc.value); // todo: -4?
            pc.set(address);
        }

        #region exceptions

        private void exception_res() { vector(Mode.SVC, 0x00000000, 1, 1); }
        private void exception_und() { vector(Mode.UND, 0x00000004, 1, cpsr.f); }
        private void exception_swi() { vector(Mode.SVC, 0x00000008, 1, cpsr.f); }
        private void exception_pab() { vector(Mode.ABT, 0x0000000c, 1, cpsr.f); }
        private void exception_dab() { vector(Mode.ABT, 0x00000010, 1, cpsr.f); }
        private void exception____() { }
        private void exception_irq() { vector(Mode.IRQ, 0x00000018, 1, cpsr.f); }
        private void exception_fiq() { vector(Mode.FIQ, 0x0000001c, 1, 1); }

        #endregion

        protected void step() { }

        public class Flags
        {
            public uint n, z, c, v;
            public uint r;
            public uint i, f, t, m;

            public void copy(Flags value)
            {
                this.n = value.n;
                this.z = value.z;
                this.c = value.c;
                this.v = value.v;
                this.r = value.r;
                this.i = value.i;
                this.f = value.f;
                this.t = value.t;
                this.m = value.m;
            }
            public void load(uint value)
            {
                this.n = (value >> 31) & 1;
                this.z = (value >> 30) & 1;
                this.c = (value >> 29) & 1;
                this.v = (value >> 28) & 1;
                this.r = (value >>  8) & 0xfffff;
                this.i = (value >>  7) & 1;
                this.f = (value >>  6) & 1;
                this.t = (value >>  5) & 1;
                this.m = (value >>  0) & 31;
            }
            public uint save()
            {
                return
                    (this.n << 31) |
                    (this.z << 30) |
                    (this.c << 29) |
                    (this.v << 28) |
                    (this.r <<  8) |
                    (this.i <<  7) |
                    (this.f <<  6) |
                    (this.t <<  5) |
                    (this.m <<  0);
            }
        }
        public class Mode
        {
            public const uint USR = 0x10;
            public const uint FIQ = 0x11;
            public const uint IRQ = 0x12;
            public const uint SVC = 0x13;
            public const uint ABT = 0x17;
            public const uint UND = 0x1b;
            public const uint SYS = 0x1f;

            public Flags spsr;
            public Register[] registers;

            public Mode(int index)
            {
                this.spsr = new Flags();
                this.registers = new Register[index];
            }
        }
        public class Register
        {
            public event Action modified;

            public uint value;

            public void set(uint value)
            {
                this.value = value;

                if (this.modified != null)
                    this.modified();
            }
        }
    }
}
