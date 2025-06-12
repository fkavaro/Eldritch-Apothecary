using System.Collections;
using UnityEngine;

public class Interrupted_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public Interrupted_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Interrupted", stackFsm) { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        //Accion no hacer nada hasta que se vaya el gato 
        // _controller.StartCoroutine(WaitForCatToLeave());
        _controller.ChangeAnimationTo(_controller.yellAnim);
    }

    public override void UpdateState()
    {
        if(_controller.annoyedByCat)
        {
            _stateMachine.Pop();
        }
    }

    public override void ExitState()
    {
    }
}
