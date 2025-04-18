using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WaitingIngredients_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public WaitingIngredients_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Waiting Ingredients", stackFsm) { }

    public override void StartState()
    {
        //Accion esperar ingredientes (Espera de 7 segundos)
        if (_behaviourController.HasIngredients())
        {
            _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.preparingPotionState));
        }
        else
        {
            _behaviourController.StartCoroutine(WaitingIngredients());
        }
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
    }
    private IEnumerator WaitingIngredients()
    {
        yield return new WaitForSeconds(3f); // Espera 3 segundos

        // Cambiar al siguiente estado (ejemplo: dejar la poci�n en la mesa)
        _stateMachine.SwitchState(_behaviourController.preparingPotionState);
    }

}
