using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// BehaviourTree 
/// </summary>
public class BehaviourTree : Node
{
    public BehaviourTree(string name = "BehaviourTree") : base(name) { }

    public override Status UpdateNode()
    {
        while (_currentChild < children.Count)
        {
            var status = children[_currentChild].UpdateNode();

            if (status != Status.Success)
                return status;

            _currentChild++;
        }
        return Status.Success;
    }
}