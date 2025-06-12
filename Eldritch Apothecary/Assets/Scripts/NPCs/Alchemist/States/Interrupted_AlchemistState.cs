using System.Collections;
using UnityEngine;

public class Interrupted_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public Interrupted_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Interrupted", stackFsm) { }
    public override void StartState()
    {
        // Changes to yell animation
        _controller.ChangeAnimationTo(_controller.yellAnim);
    }

    public override void UpdateState()
    {

    }
}
