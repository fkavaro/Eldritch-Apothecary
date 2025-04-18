using System.Collections;
using UnityEngine;

public class Interrupted_AlchemistState : AState<Alchemist, StackFiniteStateMachine<Alchemist>>
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
        while (Vector3.Distance(_controller.transform.position, ApothecaryManager.Instance.cat.transform.position) < _controller.minDistanceToCat)
        {
            yield return null; // Espera un frame antes de volver a comprobar
        }

        _stateMachine.ReturnToPreviousState(); // Vuelve al estado anterior cuando el gato se vaya
    }
}
