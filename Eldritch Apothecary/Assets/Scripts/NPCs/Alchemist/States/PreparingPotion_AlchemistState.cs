using System.Collections;
using UnityEngine;

public class PreparingPotion_AlchemistState : AState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public PreparingPotion_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Preparing potion", stackFsm) { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {

        //Accion hacer pocion (Espera de 7 segundos)
        _controller.StartCoroutine(PreparePotionCoroutine());
    }

    public override void UpdateState()
    {
    }

    private IEnumerator PreparePotionCoroutine()
    {
        yield return new WaitForSeconds(7f); // Espera 7 segundos
        _stateMachine.SwitchState(_controller.finishingPotionState); // Cambia al siguiente estado;
    }

    public override void ExitState()
    {
    }
}
