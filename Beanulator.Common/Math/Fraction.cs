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
	public class Fraction
	{
		/// <summary>
		/// Represents the standard form of 'Zero'. (0 / 1)
		/// </summary>
		public static readonly Fraction Zero = new Fraction(0, 1);
		/// <summary>
		/// Represents a close approximation of the constant 'Pi'
		/// </summary>
		public static readonly Fraction Pi   = new Fraction(104348, 33215);
		/// <summary>
		/// Represents a close approximation of the constant 'Tau'
		/// </summary>
		public static readonly Fraction Tau  = new Fraction(208696, 33215);

		public int n;
		public int d;

		public Fraction(int n, int d = 1)
		{
			if (d <= 0)
			{
				// i know zero is neither positive or negative, but that might be lost on others.
				throw new ArgumentException("Denominator must be positive, and non-zero.");
			}

			this.n = n;
			this.d = d;
		}

		public static Fraction Reduce(Fraction fraction)
		{
			int r = MathHelper.GreatestCommonDivisor(fraction.n, fraction.d);

			fraction.n /= r;
			fraction.d /= r;

			return fraction;
		}

		public override string ToString()
		{
			return string.Format("{0:N0}/{1:N0}", n, d);
		}

		public static Fraction operator +(Fraction a, Fraction b)
		{
			a.n = (a.n * b.d + b.n * a.d);
			a.d = (a.d * b.d);

			return Reduce(a);
		}
		public static Fraction operator -(Fraction a, Fraction b)
		{
			a.n = (a.n * b.d - b.n * a.d);
			a.d = (a.d * b.d);

			return Reduce(a);
		}
		public static Fraction operator *(Fraction a, Fraction b)
		{
			a.n = (a.n * b.n);
			a.d = (a.d * b.d);

			return Reduce(a);
		}
		public static Fraction operator /(Fraction a, Fraction b)
		{
			a.n = (a.n * b.d);
			a.d = (a.d * b.n);

			return Reduce(a);
		}

		public static Fraction operator +(Fraction a, int value) { return (a + new Fraction(value)); }
		public static Fraction operator -(Fraction a, int value) { return (a - new Fraction(value)); }
		public static Fraction operator *(Fraction a, int value) { return (a * new Fraction(value)); }
		public static Fraction operator /(Fraction a, int value) { return (a / new Fraction(value)); }
	}
}
