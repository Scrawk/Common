using System;
using System.Collections.Generic;

namespace Common.AI.Behavioural
{
    /// <summary>
    /// Decorator node that inverts the success/failure of its child.
    /// </summary>
    public class InverterNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The child to be inverted.
        /// </summary>
        private IBehaviourTreeNode childNode;

        public InverterNode(string name)
        {
            Name = name;
        }

        public BTSTATUS Tick()
        {
            if (childNode == null)
            {
                throw new ApplicationException("InverterNode must have a child node!");
            }

            var result = childNode.Tick();
            if (result == BTSTATUS.FAILURE)
            {
                return BTSTATUS.SUCCESS;
            }
            else if (result == BTSTATUS.SUCCESS)
            {
                return BTSTATUS.FAILURE;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            if (this.childNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to InverterNode!");
            }

            this.childNode = child;
        }
    }
}
    
