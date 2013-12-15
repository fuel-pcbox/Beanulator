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

﻿namespace Beanulator.Famicom
{
    public class Motherboard
    {
        private Cartridge cartridge;
        private R2A03 cpu = new R2A03();
        private R2C02 ppu = new R2C02();
        private byte[] nmt = new byte[2048];
        private byte[] ram = new byte[2048];

        protected void main()
        {
            bool power = true;

            while (power)
            {
                ppu.cycles++;
                cpu.cycles++;

                #region ppu bus decoding

                if (ppu.cycles == 4) // cycles will be modified in 'ppu.main'
                {
                    cpu.nmi(ppu.nmi && ppu.vbl); // put this before running ppu to delay it by 1 ppu cycle

                    ppu.thread.Enter();

                    var ab = (ushort)(ppu.pins.address & 0x3fff);

                    if (ppu.pins.enable)
                    {
                        if (ppu.pins.read)
                        {
                            cartridge.chr_read(ab, ref ppu.pins.data);

                            if (ab < 0x2000) { /* chr */ }
                            if (ab < 0x4000) { ppu.pins.data = nmt[ab & 0x7ff]; /* nmt */ }
                        }
                        else
                        {
                            cartridge.chr_write(ab, ref ppu.pins.data);

                            if (ab < 0x2000) { /* chr */ }
                            if (ab < 0x4000) { nmt[ab & 0x7ff] = ppu.pins.data; /* nmt */ }
                        }
                    }
                }

                #endregion
                #region cpu bus decoding

                if (cpu.cycles == 12) // cycles will be modified in 'cpu.main'
                {
                    cpu.thread.Enter();

                    var ab = cpu.pins.address;

                    if (cpu.pins.read)
                    {
                        cartridge.prg_read(ab, ref cpu.pins.data);

                        if (ab < 0x2000) { cpu.pins.data = ram[ab & 0x7ff]; continue; }
                        if (ab < 0x4000) { ppu.read_registers(ab, ref cpu.pins.data); continue; }
                        if (ab < 0x4018) { cpu.read_registers(ab, ref cpu.pins.data); continue; }
                    }
                    else
                    {
                        cartridge.prg_write(ab, ref cpu.pins.data);

                        if (ab < 0x2000) { ram[ab & 0x7ff] = cpu.pins.data; continue; }
                        if (ab < 0x4000) { ppu.write_registers(ab, ref cpu.pins.data); continue; }
                        if (ab < 0x4018) { cpu.write_registers(ab, ref cpu.pins.data); continue; }
                    }
                }

                #endregion
            }
        }

        protected void reset()
        {
            cpu.cycles = 0;
            cpu.res(true);

            // ppu.cycles = 0; // uncomment to not emulate cpu/ppu synchronizations
            // todo: reset ppu
            // todo: reset apu
        }
    }
}
