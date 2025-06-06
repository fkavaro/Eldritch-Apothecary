using System.Collections;
using UnityEngine;

public class WaitingIngredients_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    Shelf previousShelf;

    public WaitingIngredients_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
        : base("Waiting Ingredients", stackFsm)
    {
        _controller.SetDestinationSpot(ApothecaryManager.Instance.alchemistSeat);

    }


    public override void StartState()
    {
        _controller.newShelf = false;
        Debug.Log("Esperando a mas ingredientes");
    }

    public override void UpdateState() {
        if (_controller.HasArrivedAtDestination())
        {

            _controller.ChangeAnimationTo(_controller.sitDownAnim);

            SwitchStateAfterCertainTime(2f, _controller.pickingUpIngredientsState, _controller.waitAnim, "Pick Up Ingredients");

        }
    }
    
    public override void ExitState() { }

}
