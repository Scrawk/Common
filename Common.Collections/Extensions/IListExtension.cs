using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public static class IListExtension
    {

        public static void Shuffle<T>(this IList<T> list, Random rnd = null)
        {
            if (rnd == null)
                rnd = new Random();

            int n = list.Count;
            while (n > 1)
            {
                int k = rnd.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }

    }
}
