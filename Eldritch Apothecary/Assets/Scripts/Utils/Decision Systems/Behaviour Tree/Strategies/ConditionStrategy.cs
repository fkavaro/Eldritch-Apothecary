using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionStrategy : IStrategy
{
    readonly Func<bool> _predicate;

    public ConditionStrategy(Func<bool> predicate)
    {
        _predicate = predicate;
    }

    public Node.Status Update()
    {
        return _predicate() ? Node.Status.Success : Node.Status.Failure;
    }
}
