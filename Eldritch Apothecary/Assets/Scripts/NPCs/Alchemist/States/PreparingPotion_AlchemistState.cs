using System.Collections;
using UnityEngine;

public class PreparingPotion_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    int timeToPrepare = 0;
    public PreparingPotion_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Preparing potion", stackFsm) { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        _controller.SetDestinationSpot(ApothecaryManager.Instance.alchemistTable);
    }

    public override void UpdateState()
    {
        if (_controller.HasArrivedAtDestination())
        {
            timeToPrepare  = UnityEngine.Random.Range(_controller.timeToPrepareMin, _controller.timeToPrepareMax);
            SwitchStateAfterCertainTime(timeToPrepare,_controller.finishingPotionState, _controller.mixIngredientsAnim, "Preparing Potions");

        }
    }

}
