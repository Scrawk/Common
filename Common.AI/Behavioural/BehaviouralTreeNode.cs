using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AI.Behavioural
{
    /// <summary>
    /// Base class for behaviour tree nodes.
    /// </summary>
    public abstract class BehaviourTreeNode : IBehaviourTreeNode
    {
        /// <summary>
        /// The nodes name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        public abstract BTSTATUS Tick();
    }

}