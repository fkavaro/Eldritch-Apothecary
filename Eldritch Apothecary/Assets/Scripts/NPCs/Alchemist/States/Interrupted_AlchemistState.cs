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
        _controller.StartCoroutine(WaitForCatToLeave());
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
    }

    private IEnumerator WaitForCatToLeave()
    {
        while (_controller.CatIsBothering())
        {
            yield return null; // Espera un frame antes de volver a comprobar
        }

        _stateMachine.Pop(); // Vuelve al estado anterior cuando el gato se vaya
    }
}
