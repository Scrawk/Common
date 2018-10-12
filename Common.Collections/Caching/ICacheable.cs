using System;
using System.Collections.Generic;

namespace Common.Collections.Caching
{
    public interface ICacheable<T>
    {
        void Clear();
    }
}
