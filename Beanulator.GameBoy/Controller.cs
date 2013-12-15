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

namespace Beanulator.GameBoy
{
    public class Controller
    {
        //          COL_1 COL_2     
        //            |     |       
        // COL_1.0 -> o-----o <- COL_2.0
        //            |     |       
        // COL_1.1 -> o-----o <- COL_2.1
        //            |     |       
        // COL_1.2 -> o-----o <- COL_2.2
        //            |     |       
        // COL_1.3 -> o-----o <- COL_2.3

        public bool column1State;
        public bool column2State;
        public byte column1;
        public byte column2;

        public byte read()
        {
            int data = 0;

            if (column1State) { data |= column1; }
            if (column2State) { data |= column2; }

            return (byte)(~data);
        }
        public void write(byte data)
        {
            column1State = (data & 0x10) != 0;
            column2State = (data & 0x20) != 0;
        }

        public void update()
        {
            column1 = 0x10;
            column2 = 0x20;
        }
    }
}
