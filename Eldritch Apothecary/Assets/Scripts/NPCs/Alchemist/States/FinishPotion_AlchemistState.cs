using System.Collections;
using UnityEngine;

public class FinishPotion_AlchemistState : AState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public FinishPotion_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Finish potion", stackFsm) { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        //Accion de terminar poci�n (3 segundos mas)
        _behaviourController.StartCoroutine(FinishPotion());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return; //Evita que se inicie repetidamente
                                       //
                                       // Si termina, vuelve a esperar

    }

    public override void ExitState()
    {
    }

    private IEnumerator FinishPotion()
    {
        yield return new WaitForSeconds(3f); // Espera 3 segundos

        // Cambiar al siguiente estado (ejemplo: dejar la poci�n en la mesa)
        _stateMachine.SwitchState(_behaviourController.waitingState);
    }
}
