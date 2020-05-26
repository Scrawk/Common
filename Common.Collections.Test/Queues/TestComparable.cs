using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Collections.Test.Queues
{
    public class TestComparable : IComparable<TestComparable>
    {
        public float value;

        public TestComparable(float v)
        {
            value = v;
        }

        public int CompareTo(TestComparable other)
        {
            return value.CompareTo(other.value);
        }
    }
}
