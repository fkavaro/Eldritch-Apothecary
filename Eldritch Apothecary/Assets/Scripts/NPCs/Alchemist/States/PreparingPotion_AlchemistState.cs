using System.Collections;
using UnityEngine;

public class PreparingPotion_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    // Required Time  to elaborate a potion
    int timeToPrepare = 0;
    public PreparingPotion_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Preparing potion", stackFsm) { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        // Goes to the table
        _controller.SetDestinationSpot(ApothecaryManager.Instance.alchemistSpot);
    }

    public override void UpdateState()
    {
        if (_controller.HasArrivedAtDestination())
        {
            //Generates a random required time, between a min and max number defined by his personality
            timeToPrepare = UnityEngine.Random.Range(_controller.timeToPrepareMin, _controller.timeToPrepareMax);
            //After spending the required time , changes to finishing potion state
            SwitchStateAfterCertainTime(timeToPrepare, _controller.finishingPotionState, _controller.mixIngredientsAnim, "Preparing Potions");
        }
    }

}
