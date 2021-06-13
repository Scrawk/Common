using System;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class StringExtensions
    {
       public static string RemoveWhitespaces(this string str)
        {
            return new string(str.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}
