using System.Collections;
using UnityEngine;

public class PreparingPotion_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{

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
            SwitchStateAfterRandomTime(_controller.finishingPotionState, _controller.mixIngredientsAnim, "Preparing Potions");

        }
    }

}
