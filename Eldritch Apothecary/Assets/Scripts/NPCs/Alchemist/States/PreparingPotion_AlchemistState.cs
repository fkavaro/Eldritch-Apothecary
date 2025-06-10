using System.Collections;
using UnityEngine;

public class PreparingPotion_AlchemistState : AState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
<<<<<<< Updated upstream
=======
    int timeToPrepare = 0;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
        if (_controller.HasArrivedAtDestination())
        {
            timeToPrepare  = UnityEngine.Random.Range(_controller.timeToPrepareMin, _controller.timeToPrepareMax);
            SwitchStateAfterCertainTime(timeToPrepare,_controller.finishingPotionState, _controller.mixIngredientsAnim, "Preparing Potions");

        }
>>>>>>> Stashed changes
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
