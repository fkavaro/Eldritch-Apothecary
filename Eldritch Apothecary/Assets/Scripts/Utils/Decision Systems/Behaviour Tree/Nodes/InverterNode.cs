using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// InverterNode is a logical node that inverts its only child status.
/// Like a logical NOR operation, it will return success when the child returns failure.
/// </summary>
public class InverterNode<TController> : Node<TController>
where TController : ABehaviourController<TController>
{
    public InverterNode(TController controller, string name) : base(controller, name) { }

    public override Status UpdateNode()
    {
        switch (children[0].UpdateNode())
        {
            case Status.Running:
                return Status.Running;
            case Status.Failure:
                return Status.Success;
            default:
                return Status.Failure;
        }
    }
}