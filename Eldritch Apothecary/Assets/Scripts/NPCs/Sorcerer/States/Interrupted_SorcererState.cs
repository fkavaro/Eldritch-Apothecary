using System.Collections;
using UnityEngine;

public class Interrupted_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    public Interrupted_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
    : base("Interrupted", sfsm) { }

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
