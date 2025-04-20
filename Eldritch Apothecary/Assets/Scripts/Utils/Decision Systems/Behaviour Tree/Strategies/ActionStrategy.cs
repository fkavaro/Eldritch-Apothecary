using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionStrategy<TController> : IStrategy<TController>
where TController : ABehaviourController<TController>
{
    readonly Action _action;

    public ActionStrategy(Action action)
    {
        _action = action;
    }

    public Node<TController>.Status Update()
    {
        _action();
        return Node<TController>.Status.Success;
    }
}