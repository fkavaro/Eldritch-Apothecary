using System.Collections;
using UnityEngine;

public class Interrupted_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public Interrupted_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Interrupted", stackFsm) { }

    public override void StartState()
    {
        _controller.SetIfStopped(true);
        _controller.ChangeAnimationTo(_controller.yellAnim);
    }

    public override void UpdateState()
    {
        _controller.transform.LookAt(ApothecaryManager.Instance.cat.transform.position);
    }

    public override void ExitState()
    {
        _controller.SetIfStopped(false);
    }
}
