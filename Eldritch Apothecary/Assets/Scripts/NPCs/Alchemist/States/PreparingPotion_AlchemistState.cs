using System.Collections;
using UnityEngine;

public class PreparingPotion_AlchemistState : AState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public PreparingPotion_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm) : base(stackFsm)
    {
        stateName = "Preparing potion";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {

        //Accion hacer pocion (Espera de 7 segundos)
        _behaviourController.StartCoroutine(PreparePotionCoroutine());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return; //Evita que se inicie repetidamente
    }

    private IEnumerator PreparePotionCoroutine()
    {
        yield return new WaitForSeconds(7f); // Espera 7 segundos
        _stateMachine.SwitchState(_behaviourController.finishingPotionState); // Cambia al siguiente estado;
    }

    public override void ExitState()
    {
    }
}
