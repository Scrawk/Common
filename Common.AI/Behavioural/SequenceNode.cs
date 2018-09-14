using System;
using System.Collections.Generic;

namespace Common.AI.Behavioural
{

    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>();

        public SequenceNode(string name)
        {
            Name = name;
        }

        public BTSTATUS Tick()
        {
            foreach (var child in children)
            {
                var childStatus = child.Tick();
                if (childStatus != BTSTATUS.SUCCESS)
                {
                    return childStatus;
                }
            }

            return BTSTATUS.SUCCESS;
        }

        /// <summary>
        /// Add a child to the sequence.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}