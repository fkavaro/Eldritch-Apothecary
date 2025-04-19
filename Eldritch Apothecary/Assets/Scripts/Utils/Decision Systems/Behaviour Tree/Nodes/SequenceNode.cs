using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// SequenceNode is a composite node that executes its children in sequence.
/// Like a logical AND operation, it will return success only if all its children return success.
/// </summary>
public class SequenceNode : Node
{
    public SequenceNode(string name, int priority = 0) : base(name, priority) { }

    public override Status UpdateNode()
    {
        // Execute every child
        if (_currentChild < children.Count)
        {
            switch (children[_currentChild].UpdateNode())
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failure:
                    Reset();
                    return Status.Failure;
                default: // Success
                    _currentChild++; // Next one
                    // Success if it was the last, if not continue
                    return _currentChild == children.Count ? Status.Success : Status.Running;
            }
        }
        Reset();
        return Status.Success;
    }
}