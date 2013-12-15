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

using Beanulator.Common.Processors.SHARP;

namespace Beanulator.GameBoy
{
    public class CPU : LR35902
    {
        private byte[] hram = new byte[0x0080]; // $ff80 - $fffe
        private byte[] vram = new byte[0x2000]; // $8000 - $9fff
        private byte[] oram = new byte[0x00a0]; // $fe00 - $fea0
        private byte[] wram = new byte[0x2000]; // $c000 - $dfff
        private dynamic cart; // cartridge connector

        protected override void main()
        {
            while (true)
            {
                base.step();
            }
        }
        protected override void read()
        {
            pins.data = cart.read(pins.address);

            /**/ if (pins.address >= 0x8000 && pins.address <= 0x9fff) { pins.data = vram[pins.address & 0x1fff]; }
            else if (pins.address >= 0xc000 && pins.address <= 0xfdff) { pins.data = wram[pins.address & 0x1fff]; }
            else if (pins.address >= 0xfe00 && pins.address <= 0xfe9f) { pins.data = oram[pins.address & 0x00ff]; }
            else if (pins.address >= 0xff00 && pins.address <= 0xff7f) { /* i/o */ }
            else if (pins.address >= 0xff80 && pins.address <= 0xfffe) { pins.data = hram[pins.address & 0x007f]; }
            else if (pins.address == 0xffff) { pins.data = interrupts.rf; }
        }
        protected override void write()
        {
            cart.write(pins.address, pins.data);

            /**/ if (pins.address >= 0x8000 && pins.address <= 0x9fff) { vram[pins.address & 0x1fff] = pins.data; }
            else if (pins.address >= 0xc000 && pins.address <= 0xfdff) { wram[pins.address & 0x1fff] = pins.data; }
            else if (pins.address >= 0xfe00 && pins.address <= 0xfe9f) { oram[pins.address & 0x00ff] = pins.data; }
            else if (pins.address >= 0xff00 && pins.address <= 0xff7f) { /* i/o */ }
            else if (pins.address >= 0xff80 && pins.address <= 0xfffe) { hram[pins.address & 0x007f] = pins.data; }
            else if (pins.address == 0xffff) { interrupts.rf = pins.data; }
        }
    }
}
