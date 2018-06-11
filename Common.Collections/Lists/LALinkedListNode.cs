using System;
using System.Collections.Generic;

namespace Common.Collections.Lists
{
    public class LALinkedListNode<TYPE> where TYPE : class
    {
        public TYPE Value { get; set; }
        public LALinkedListNode<TYPE> Next { get; set; }
    }
}
