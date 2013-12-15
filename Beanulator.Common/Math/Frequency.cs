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
	public class Frequency
	{
		private Fraction r;
		private int f;
		private int cycles;
		private int single;
		private int period;

		public Frequency(int f, Fraction r)
		{
			// check for improper/whole fractions
			if (r.n >= r.d)
				throw new ArgumentException("Improper and Whole fractions aren't allowed when defining clock source frequency.");

			if (r.d <= 0)
				throw new ArgumentException("Denominator must be positive, and non-zero.");

			this.f = f;
			this.r = r;
		}

		public static void Synchronize(Frequency a, Frequency b)
		{
			if (a.r.d <= 0) throw new ArgumentException("Denominator of 'a' must be positive, and non-zero.");
			if (b.r.d <= 0) throw new ArgumentException("Denominator of 'b' must be positive, and non-zero.");

			// make both frequencies integers, and align them to each other
			// todo: make 64-bit?
			a.single = ((a.f * a.r.d) + a.r.n) * b.r.d;
			b.single = ((b.f * b.r.d) + b.r.n) * a.r.d;

			int gcd = MathHelper.GreatestCommonDivisor(a.single, b.single);

			if (gcd != 0) // avoid division by zero
			{
				a.single /= gcd;
				b.single /= gcd;
			}

			// create synchronization point
			if (a.single > b.single)
			{
				a.period = a.single;
				b.period = a.single;
			}
			else
			{
				a.period = b.single;
				b.period = b.single;
			}
		}

		public bool Step()
		{
			cycles += single;

			if (cycles >= period)
			{
				cycles -= period;
				return true;
			}

			return false;
		}

		public override string ToString()
		{
			return string.Format("{0:N0}+{1}", f, r);
		}
	}
}
