using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AI.Behavioural
{
    /// <summary>
    /// Runs childs nodes in parallel.
    /// </summary>
    public class ParallelNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>();

        /// <summary>
        /// Number of child failures required to terminate with failure.
        /// </summary>
        private int numRequiredToFail;

        /// <summary>
        /// Number of child successess require to terminate with success.
        /// </summary>
        private int numRequiredToSucceed;

        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed)
        {
            Name = name;
            this.numRequiredToFail = numRequiredToFail;
            this.numRequiredToSucceed = numRequiredToSucceed;
        }

        public BTSTATUS Tick()
        {
            var numChildrenSuceeded = 0;
            var numChildrenFailed = 0;

            foreach (var child in children)
            {
                var childStatus = child.Tick();
                switch (childStatus)
                {
                    case BTSTATUS.SUCCESS: ++numChildrenSuceeded; break;
                    case BTSTATUS.FAILURE: ++numChildrenFailed; break;
                }
            }

            if (numRequiredToSucceed > 0 && numChildrenSuceeded >= numRequiredToSucceed)
            {
                return BTSTATUS.SUCCESS;
            }

            if (numRequiredToFail > 0 && numChildrenFailed >= numRequiredToFail)
            {
                return BTSTATUS.FAILURE;
            }

            return BTSTATUS.RUNNING;
        }

        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}