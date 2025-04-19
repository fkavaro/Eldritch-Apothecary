using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionStrategy : IStrategy
{
    readonly Action _action;

    public ActionStrategy(Action action)
    {
        _action = action;
    }

    public Node.Status Update()
    {
        _action();
        return Node.Status.Success;
    }
}