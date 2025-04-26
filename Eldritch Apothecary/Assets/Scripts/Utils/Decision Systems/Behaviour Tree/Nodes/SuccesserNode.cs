using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SuccesserNode<TController> : Node<TController>
where TController : ABehaviourController<TController>
{
    public SuccesserNode(TController controller) : base(controller, "Successer") { }

    public override Status UpdateNode()
    {
        switch (children[0].UpdateNode())
        {
            case Status.Running:
                return Status.Running;
            default: // Always return success
                return Status.Success;
        }
    }
}