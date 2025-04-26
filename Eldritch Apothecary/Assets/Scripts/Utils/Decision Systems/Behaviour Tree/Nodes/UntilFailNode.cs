using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// UntilFailNode is a node that continues running its only child as long as it doesn't return failure.
/// </summary>
public class UntilFailNode<TController> : Node<TController>
where TController : ABehaviourController<TController>
{
    private readonly Node<TController> _child; // Make sure we have a reference to the child

    public UntilFailNode(TController controller, Node<TController> child) : base(controller, "UntilFail")
    {
        AddChild(child); // Use the AddChild method to set the child
        _child = children[0]; // Store a direct reference for easier access
    }

    public override Status UpdateNode()
    {
        if (_child.UpdateNode() == Status.Failure)
        {
            Reset();
            return Status.Failure;
        }
        return Status.Running;
    }
}
