using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.AI.Behavioural
{
    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public interface IBehaviourTreeNode
    {
        /// <summary>
        /// The nodes name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        BTSTATUS Tick();
    }

    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public interface IParentBehaviourTreeNode : IBehaviourTreeNode
    {
        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        void AddChild(IBehaviourTreeNode child);
    }
}