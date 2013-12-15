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

namespace Beanulator.Common
{
    using half = System.UInt16;
    using word = System.UInt32;

    public static class Utility
    {
        public static int BitsSet(byte value)
        {
            int count = 0;

            while (value != 0)
            {
                value &= (byte)(value - 1);
                count++;
            }

            return count;
        }
        public static int BitsSet(half value)
        {
            int count = 0;

            while (value != 0)
            {
                value &= (half)(value - 1);
                count++;
            }

            return count;
        }
        public static int BitsSet(word value)
        {
            int count = 0;

            while (value != 0)
            {
                value &= (word)(value - 1);
                count++;
            }

            return count;
        }

        public static void Initialize<T>(this T[] array)
            where T : new()
        {
            array.Initialize(() => new T());
        }
        public static void Initialize<T>(this T[] array, Func<T> factory)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = factory();
            }
        }

        public static uint Pattern(string value, uint bit1, uint bit0, uint bitx)
        {
            uint result = 0;

            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                case ' ': break;
                case '0': result = (result << 1) | bit0; break;
                case '1': result = (result << 1) | bit1; break;
                default : result = (result << 1) | bitx; break;
                }
            }

            return result;
        }
    }
}
