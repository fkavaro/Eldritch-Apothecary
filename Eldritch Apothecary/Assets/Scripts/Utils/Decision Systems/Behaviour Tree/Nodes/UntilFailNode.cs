using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// UntilFailNode is a node that continues running its only child as long as it doesn't return failure.
/// </summary>
public class UntilFailNode : Node
{
    public UntilFailNode(string name) : base(name) { }

    public override Status UpdateNode()
    {
        if (children[0].UpdateNode() == Status.Failure)
        {
            Reset();
            return Status.Failure;
        }
        return Status.Running;
    }
}
