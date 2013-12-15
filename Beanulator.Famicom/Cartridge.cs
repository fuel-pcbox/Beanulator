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

namespace Beanulator.Famicom
{
    //           ┌─────────┐
    // GND       │ 01   31 │ +5V
    // PRG A11   │ 02   32 │ ϕ2
    // PRG A10   │ 03   33 │ PRG A12
    // PRG A9    │ 04   34 │ PRG A13
    // PRG A8    │ 05   35 │ PRG A14
    // PRG A7    │ 06   36 │ PRG D7
    // PRG A6    │ 07   37 │ PRG D6
    // PRG A5    │ 08   38 │ PRG D5
    // PRG A4    │ 09   39 │ PRG D4
    // PRG A3    │ 10   40 │ PRG D3
    // PRG A2    │ 11   41 │ PRG D2
    // PRG A1    │ 12   42 │ PRG D1
    // PRG A0    │ 13   43 │ PRG D0
    // PRG R/W   │ 14   44 │ PRG /CE (/A15 + /M2)
    // /IRQ      │ 15   45 │ Audio from 2A03
    // GND       │ 16   46 │ Audio to RF
    // CHR /RD   │ 17   47 │ CHR /WR
    // CIRAM A10 │ 18   48 │ CIRAM /CE
    // CHR A6    │ 19   49 │ CHR /A13
    // CHR A5    │ 20   50 │ CHR A7
    // CHR A4    │ 21   51 │ CHR A8
    // CHR A3    │ 22   52 │ CHR A9
    // CHR A2    │ 23   53 │ CHR A10
    // CHR A1    │ 24   54 │ CHR A11
    // CHR A0    │ 25   55 │ CHR A12
    // CHR D0    │ 26   56 │ CHR A13
    // CHR D1    │ 27   57 │ CHR D7
    // CHR D2    │ 28   58 │ CHR D6
    // CHR D3    │ 29   59 │ CHR D5
    // +5V       │ 30   60 │ CHR D4
    //           └─────────┘

    public abstract class Cartridge
    {
        public Pins pins;

        protected abstract void chr_read();
        protected abstract void chr_write();
        protected abstract void prg_read();
        protected abstract void prg_write();

        public void chr_read(ushort address, ref byte data)
        {
            pins.chr_address = address;

            chr_read();

            data = pins.chr_data;
        }
        public void chr_write(ushort address, ref byte data)
        {
            pins.chr_address = address;
            pins.chr_data = data;

            chr_write();
        }
        public void prg_read(ushort address, ref byte data)
        {
            pins.prg_address = address;

            prg_read();

            data = pins.prg_data;
        }
        public void prg_write(ushort address, ref byte data)
        {
            pins.prg_address = address;
            pins.prg_data = data;

            prg_write();
        }

        public struct Pins
        {
            public bool irq;			// interrupt request
            public ushort chr_address;	// 2c02 address
            public ushort prg_address;	// 2a03 address
            public byte chr_data;		// 2c02 data
            public byte prg_data;		// 2a03 data
        }
    }
}
