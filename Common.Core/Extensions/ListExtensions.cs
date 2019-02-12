using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class ListExtension
    {
        public static void Add<T>(this List<T> list, T item1, T item2)
        {
            list.Add(item1);
            list.Add(item2);
        }

        public static void Add<T>(this List<T> list, T item1, T item2, T item3)
        {
            list.Add(item1);
            list.Add(item2);
            list.Add(item3);
        }
    }
}
