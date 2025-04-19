using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Base class for all nodes in the behaviour tree.
/// </summary>
public class Node
{
    public enum Status
    {
        Running, // In progress
        Success,
        Failure
    }

    public readonly string name;
    public readonly int priority;

    public readonly List<Node> children = new();
    protected int _currentChild;

    public Node(string name = "Node", int priority = 0)
    {
        this.name = name;
        this.priority = priority;
    }

    public void AddChild(Node child)
    {
        children.Add(child);
    }

    public virtual Status UpdateNode()
    {
        return children[_currentChild].UpdateNode();
    }

    public virtual void Reset()
    {
        _currentChild = 0;
        foreach (var child in children)
        {
            child.Reset();
        }
    }
}
