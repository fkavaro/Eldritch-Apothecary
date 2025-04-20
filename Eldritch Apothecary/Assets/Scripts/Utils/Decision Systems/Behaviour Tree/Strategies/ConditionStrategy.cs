using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionStrategy<TController> : IStrategy<TController>
where TController : ABehaviourController<TController>
{
    readonly Func<bool> _predicate;

    public ConditionStrategy(Func<bool> predicate)
    {
        _predicate = predicate;
    }

    public Node<TController>.Status Update()
    {
        return _predicate() ? Node<TController>.Status.Success : Node<TController>.Status.Failure;
    }
}
