using System.Collections;
using UnityEngine;

public class WaitingIngredients_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    // Saves last used shelf to check the same shelf
    Shelf previousShelf;

    public WaitingIngredients_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
        : base("Waiting Ingredients", stackFsm)
    {
        // Goes to his seat
        _controller.SetDestinationSpot(ApothecaryManager.Instance.alchemistSeat);
    }


    public override void StartState()
    {
        _controller.newShelf = false;
    }

    public override void UpdateState() {
        if (_controller.HasArrivedAtDestination())
        {
            // Sit down
            _controller.ChangeAnimationTo(_controller.sitDownAnim);
            // After 2 seconds sitting, changes to pick up ingredients state to check the shelf
            SwitchStateAfterCertainTime(2f, _controller.pickingUpIngredientsState, _controller.waitAnim, "Pick Up Ingredients");

        }
    }
   
}
