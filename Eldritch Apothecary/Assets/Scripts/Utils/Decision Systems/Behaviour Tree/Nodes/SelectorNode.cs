using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// SelectorNode is a composite node that that executes its children in sequence.
/// Like a logical OR operation, it will return success when a child return success.
/// </summary>
public class SelectorNode : Node
{
    public SelectorNode(string name, int priority = 0) : base(name, priority) { }

    public override Status UpdateNode()
    {
        // Execute every child
        if (_currentChild < children.Count)
        {
            switch (children[_currentChild].UpdateNode())
            {
                case Status.Running:
                    return Status.Running;
                case Status.Success:
                    Reset();
                    return Status.Success;
                default: // Failure
                    _currentChild++; // Next one
                    // Success if it was the last, if not continue
                    return _currentChild == children.Count ? Status.Success : Status.Running;
            }
        }
        Reset();
        return Status.Failure;
    }
}