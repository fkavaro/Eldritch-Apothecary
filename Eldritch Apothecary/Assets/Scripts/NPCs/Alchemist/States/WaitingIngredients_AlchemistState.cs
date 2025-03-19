using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class WaitingIngredients_AlchemistState : AnAlchemistState
{
    public WaitingIngredients_AlchemistState(StackFiniteStateMachine stackFsm, Alchemist alchemistContext) : base(stackFsm, alchemistContext)
    {
        stateName = "Waiting Ingredients";
    }
    public override void StartState()
    {
        //Accion esperar ingredientes (Espera de 7 segundos)
        if (_alchemistContext.HasIngredients())
        {
            _alchemistContext.StartCoroutine(WaitAndSwitchState(_alchemistContext.preparingPotionState, "Preparing potion"));
        }
        else
        {
            _alchemistContext.StartCoroutine(WaitingIngredients());

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

        // Cambiar al siguiente estado (ejemplo: dejar la poción en la mesa)
        _stackFsm.SwitchState(_alchemistContext.preparingPotionState);
    }

}
