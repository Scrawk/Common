using System;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace Common.Core.Numerics
{
    /// <summary>
    /// Integer math functions.
    /// </summary>
	public class IMath 
	{

        public const int MAX_FACTORIAL = 20;

        private static ulong[] m_factorialTable;

        /// <summary>
        /// Clamp a value between min and max (inclusive).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int v, int min, int max)
        {
            if (v < min) v = min;
            if (v > max) v = max;
            return v;
        }

        /// <summary>
        /// Wrap a value between 0 and count-1 (inclusive).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Wrap(int v, int count)
        {
            int r = v % count;
            return r < 0 ? r + count : r;
        }

        /// <summary>
        /// Mirror a value between 0 and count-1 (inclusive).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Mirror(int v, int count)
        {
            int m1 = count - 1;
            int i = Math.Abs(v);

            v = i % (m1 * 2);
            if (v >= m1) v = m1 - i % m1;

            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Sqr(int v)
        {
            return v * v;
        }

        /// <summary>
        /// Is number a power of 2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPow2(int num)
        {
            int power = (int)(Math.Log(num) / Math.Log(2.0));
            double result = Math.Pow(2.0, power);
            return result == num;
        }

        /// <summary>
        /// Return the closest pow2 number to num.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NearestPow2(int num)
		{
			int n = num > 0 ? num - 1 : 0;
			
			n |= n >> 1;
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;
			n++;
			
			return n;
		}

        /// <summary>
        /// Return the closest pow2 number thats less than num.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LowerPow2(int num)
		{
			int n = num > 0 ? num - 1 : 0;

			n |= n >> 1;
			n |= n >> 2;
			n |= n >> 4;
			n |= n >> 8;
			n |= n >> 16;
			n++;

			return n - (n >> 1);
		}

        /// <summary>
        /// Simple int pow function.
        /// System Math.Pow may produce precision errors.
        /// </summary>
        public static long Pow(int a, int b)
        {
            if (b < 0) throw new ArgumentException("Exponent must be > 0");
            checked
            {
                if (b == 2) return a * a;
                if (b == 3) return a * a * a;

                long p = 1;
                for (int i = 0; i < b; i++)
                    p *= a;
                return p;
            }
        }

        public static ulong Pow(uint a, uint b)
        {
            checked
            {
                if (b == 2) return a * a;
                if (b == 3) return a * a * a;

                ulong p = 1;
                for (uint i = 0; i < b; i++)
                    p *= a;
                return p;
            }
        }

        /// <summary>
        /// Returns the factorial of number.
        /// Must be less than or equal MAX_FACTORIAL or overflow will occur.
        /// </summary>
        public static ulong Factorial(int num)
        {
            if (num <= 1) return 1;
            if (num > MAX_FACTORIAL)
                throw new ArgumentException("num > MAX_FACTORIAL");

            CreateFactorialTable();
            return m_factorialTable[num];

        }

        /// <summary>
        /// Returns the factorial of number using a BigInteger.
        /// </summary>
        public static BigInteger FactorialBI(int num)
        {
            if (num <= MAX_FACTORIAL)
                return Factorial(num);

            BigInteger f = Factorial(MAX_FACTORIAL);
            for (int i = MAX_FACTORIAL; i <= num; i++)
                f = f * i;

            return f;
        }

        /// <summary>
        /// Given N objects, how many unique sets exist.
        /// </summary>
        public static BigInteger Permutations(int N)
        {
            return FactorialBI(N);
        }

        /// <summary>
        /// Given N objects, how many unique sets exist of size n 
        /// where the order matters and objects may repeat.
        /// </summary>
        /// <param name="n">The size of the sets</param>
        /// <param name="N">The total number of objects</param>
        /// <returns>The number of sets possible</returns>
        public static BigInteger PermutationsOrderedWithRepeats(int n, int N)
        {
            return BigInteger.Pow(N, n);
        }

        /// <summary>
        /// Given N objects, how many unique sets exist of size n 
        /// where the order does not matters and objects may repeat.
        /// </summary>
        /// <param name="n">The size of the sets</param>
        /// <param name="N">The total number of objects</param>
        /// <returns>The number of sets possible</returns>
        public static BigInteger PermutationsUnorderedWithRepeats(int n, int N)
        {
            var a = FactorialBI(n + N - 1);
            var b = FactorialBI(n);
            var c = FactorialBI(N - 1);

            return a / (b * c);
        }

        /// <summary>
        /// Given N objects, how many unique sets exist of size n 
        /// where the order matters and objects may not repeat.
        /// </summary>
        /// <param name="n">The size of the sets</param>
        /// <param name="N">The total number of objects</param>
        /// <returns>The number of sets possible</returns>
        public static BigInteger PermutationsOrderedWithoutRepeats(int n, int N)
        {
            var a = FactorialBI(N);
            var b = FactorialBI(N - n);

            return a / b;
        }

        /// <summary>
        /// Given N objects, how many unique sets exist of size n 
        /// where the order does not matters and objects may not repeat.
        /// </summary>
        /// <param name="n">The size of the sets</param>
        /// <param name="N">The total number of objects</param>
        /// <returns>The number of sets possible</returns>
        public static BigInteger PermutationsUnorderedWithoutRepeats(int n, int N)
        {
            var a = FactorialBI(N);
            var b = FactorialBI(n);
            var c = FactorialBI(N - n);

            return a / (b * c);
        }

        /// <summary>
        /// Creates a look up table for factorials.
        /// </summary>
        private static void CreateFactorialTable()
        {
            if (m_factorialTable != null) return;

            m_factorialTable = new ulong[MAX_FACTORIAL+1];
            m_factorialTable[0] = 1;

            ulong f = 1;
            for (ulong i = 1; i < (ulong)m_factorialTable.Length; i++)
            {
                f = f * i;
                m_factorialTable[i] = f;
            }
        }

    }
}




















