using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// LeafNode is a node that has no children and executes an specific action.
/// </summary>
public class LeafNode : Node
{
    readonly IStrategy _strategy;

    public LeafNode(string name, IStrategy strategy, int priority = 0) : base(name, priority)
    {
        _strategy = strategy;
    }

    public override Status UpdateNode()
    {
        return _strategy.Update();
    }

    public override void Reset()
    {
        _strategy.Reset();
    }
}