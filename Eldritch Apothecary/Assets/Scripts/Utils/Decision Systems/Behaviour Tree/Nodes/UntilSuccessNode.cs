using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// UntilSuccessNode is a node that continues running its only child as long as it doesn't return success.
/// </summary>
public class UntilSuccessNode : Node
{
    public UntilSuccessNode(string name) : base(name) { }

    public override Status UpdateNode()
    {
        if (children[0].UpdateNode() == Status.Success)
        {
            Reset();
            return Status.Success;
        }
        return Status.Running;
    }
}