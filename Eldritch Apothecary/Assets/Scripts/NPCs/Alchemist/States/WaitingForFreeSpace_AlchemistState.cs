using System.Collections;
using UnityEngine;

public class WaitingForFreeSpace_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public WaitingForFreeSpace_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Waiting For Free Space", stackFsm) { }

    public override void StartState()
    {
        // Goes to alchemist Table
        _controller.ChangeAnimationTo(_controller.idleAnim);
    }

    public override void UpdateState()
    {
        // After 2 seconds, restart finish a potion state to check if there is a new empty spot
        SwitchStateAfterCertainTime(2f, _controller.finishingPotionState, _controller.pickUpAnim, "Finishing Potions");

    }
   
}
