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

using Beanulator.Common;

namespace Beanulator.SuperFamicom
{
    public class Motherboard
    {
        Frequency clk1;
        Frequency clk2;

        SCPU  cpu;
        SDSP  dsp;
        SPPU1 ppu1;
        SPPU2 ppu2;
        SSMP  smp;

        public Motherboard()
        {
            clk1 = new Frequency(21477272, new Fraction(8, 11)); // 21,477,272.72~
            clk2 = new Frequency(24576000, new Fraction(0,  0)); // 24,576,000.00

            Frequency.Synchronize(clk1, clk2);

            cpu  = new SCPU ();
            dsp  = new SDSP ();
            ppu1 = new SPPU1();
            ppu2 = new SPPU2();
            smp  = new SSMP ();
        }

        protected void main()
        {
            while (true)
            {
                if (clk1.Step())
                {
                    cpu .cycles++;
                    ppu1.cycles++;
                    ppu2.cycles++;

                    if (cpu .cycles == 6) cpu .thread.Enter();
                    if (ppu1.cycles == 4) ppu1.thread.Enter();
                    if (ppu2.cycles == 4) ppu2.thread.Enter();
                }

                if (clk2.Step())
                {
                    dsp.cycles++;
                    smp.cycles++;

                    if (dsp.cycles == 12) dsp.thread.Enter();
                    if (smp.cycles == 24) smp.thread.Enter();
                }
            }
        }
    }
}
