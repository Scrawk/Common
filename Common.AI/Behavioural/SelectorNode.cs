using System;
using System.Collections.Generic;

namespace Common.AI.Behavioural
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>();

        public SelectorNode(string name)
        {
            Name = name;
        }

        public BTSTATUS Tick()
        {
            foreach (var child in children)
            {
                var childStatus = child.Tick();
                if (childStatus != BTSTATUS.FAILURE)
                {
                    return childStatus;
                }
            }

            return BTSTATUS.FAILURE;
        }

        /// <summary>
        /// Add a child node to the selector.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}