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

using Beanulator.Common.Processors.MOS;
using System;

namespace Beanulator.Famicom
{
    public class R2A03 : R6502
    {
        // todo: determine input/output property of each pin
        //       ┌─────┐┌─────┐
        //       │ ○   └┘     │
        // SND1 ─┤ 01      40 ├─ +5V (Vcc)
        // SND2 ─┤ 02      39 ├─ STROBE (joypad)
        // /RST ─┤ 03      38 ├─ EXT 44
        //   A0 ─┤ 04      37 ├─ EXT 45
        //   A1 ─┤ 05      36 ├─ /OE (joypad 0)
        //   A2 ─┤ 06      35 ├─ /OE (joypad 1)
        //   A3 ─┤ 07      34 ├─ R/W
        //   A4 ─┤ 08      33 ├─ /NMI
        //   A5 ─┤ 09      32 ├─ /IRQ
        //   A6 ─┤ 10      31 ├─ M2
        //   A7 ─┤ 11      30 ├─ GND*
        //   A8 ─┤ 12      29 ├─ CLK
        //   A9 ─┤ 13      28 ├─ D0
        //  A10 ─┤ 14      27 ├─ D1
        //  A11 ─┤ 15      26 ├─ D2
        //  A12 ─┤ 16      25 ├─ D3
        //  A13 ─┤ 17      24 ├─ D4
        //  A14 ─┤ 18      23 ├─ D5
        //  A15 ─┤ 19      22 ├─ D6
        //  GND ─┤ 20      21 ├─ D7
        //       └────────────┘
        private sq1_channel sq1;
        private sq2_channel sq2;
        private tri_channel tri;
        private noi_channel noi;
        private pcm_channel pcm;

        protected override void main()
        {
            while (true)
            {
                base.step();
            }
        }
        protected override void tick(int cycles)
        {
            sq1.cycles += cycles;
            sq2.cycles += cycles;
            tri.cycles += cycles;
            noi.cycles += cycles;
            pcm.cycles += cycles;

            while (sq1.cycles > sq1.period) { sq1.cycles -= (sq1.period + 1); /* todo: clock sq1 */ }
            while (sq2.cycles > sq2.period) { sq2.cycles -= (sq2.period + 1); /* todo: clock sq2 */ }
            while (tri.cycles > tri.period) { tri.cycles -= (tri.period + 1); /* todo: clock tri */ }
            while (noi.cycles > noi.period) { noi.cycles -= (noi.period + 1); /* todo: clock noi */ }
            while (pcm.cycles > pcm.period) { pcm.cycles -= (pcm.period + 1); /* todo: clock pcm */ }

            base.tick(cycles);
        }

        public void read_registers(ushort address, ref byte data) { }
        public void write_registers(ushort address, ref byte data) { }

        private struct duration
        {
        }

        private struct envelope
        {
        }

        private struct sq1_channel : ISoundChannel
        {
            public duration duration;
            public envelope envelope;
            public int cycles;
            public int period;

            public byte sample()
            {
                throw new NotImplementedException();
            }
        }

        private struct sq2_channel : ISoundChannel
        {
            public duration duration;
            public envelope envelope;
            public int cycles;
            public int period;

            public byte sample()
            {
                throw new NotImplementedException();
            }
        }

        private struct tri_channel : ISoundChannel
        {
            public duration duration;
            public int cycles;
            public int period;

            public byte sample()
            {
                throw new NotImplementedException();
            }
        }

        private struct noi_channel : ISoundChannel
        {
            public duration duration;
            public envelope envelope;
            public int cycles;
            public int period;

            public byte sample()
            {
                throw new NotImplementedException();
            }
        }

        private struct pcm_channel : ISoundChannel
        {
            public int cycles;
            public int period;

            public byte sample()
            {
                throw new NotImplementedException();
            }
        }

        public interface ISoundChannel
        {
            byte sample();
        }
    }
}
