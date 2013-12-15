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

using Beanulator.Common.Processors;
using System;

namespace Beanulator.Famicom
{
    public class R2C02 : Processor
    {
        //          ┌─────┐┌─────┐
        //          │ ○   └┘     │
        //  R/W  → ─┤ 01      40 ├─ ←  VCC
        //   D0 ←→ ─┤ 02      39 ├─  → ALE
        //   D1 ←→ ─┤ 03      38 ├─ ←→ AD0
        //   D2 ←→ ─┤ 04      37 ├─ ←→ AD1
        //   D3 ←→ ─┤ 05      36 ├─ ←→ AD2
        //   D4 ←→ ─┤ 06      35 ├─ ←→ AD3
        //   D5 ←→ ─┤ 07      34 ├─ ←→ AD4
        //   D6 ←→ ─┤ 08      33 ├─ ←→ AD5
        //   D7 ←→ ─┤ 09      32 ├─ ←→ AD6
        //   A2  → ─┤ 10      31 ├─ ←→ AD7
        //   A1  → ─┤ 11      30 ├─  → A8
        //   A0  → ─┤ 12      29 ├─  → A9
        //  /CS  → ─┤ 13      28 ├─  → A10
        // EXT0 ←→ ─┤ 14      27 ├─  → A11
        // EXT1 ←→ ─┤ 15      26 ├─  → A12
        // EXT2 ←→ ─┤ 16      25 ├─  → A13
        // EXT3 ←→ ─┤ 17      24 ├─  → /R
        //  CLK  → ─┤ 18      23 ├─  → /W
        // /VBL ←  ─┤ 19      22 ├─ ←  /SYNC
        //  VEE  → ─┤ 20      21 ├─  → VOUT
        //          └────────────┘

        public Pins pins;
        public bool nmi;
        public bool vbl;

        protected override void main() { }

        public void read_registers(ushort address, ref byte data) { }
        public void write_registers(ushort address, ref byte data) { }

        public struct Pins
        {
            public bool enable;     //  1-bit: bus enable
            public bool read;       //  1-bit: read/write select
            public ushort address;  // 14-bit: multiplexed address/data bus (ad0-ad7,a8-a13)
            public byte data;       //  8-bit: multiplexed address/data bus (ad0-ad7)
            public byte ext;        //  4-bit: backdrop palette index
        }
    }
}
