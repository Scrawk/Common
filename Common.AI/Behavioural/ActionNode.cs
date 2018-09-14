using System;
using System.Collections.Generic;

namespace Common.AI.Behavioural
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode : IBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private Func<BTSTATUS> fn;

        public ActionNode(string name, Func<BTSTATUS> fn)
        {
            Name = name;
            this.fn = fn;
        }

        public BTSTATUS Tick()
        {
            return fn();
        }
    }
}