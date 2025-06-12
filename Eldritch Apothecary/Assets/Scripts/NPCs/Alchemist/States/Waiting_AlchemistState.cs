using UnityEngine;

public class Waiting_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public Waiting_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Waiting", stackFsm) { }

    public override void StartState()
    {
        // Goes to alchemist seat
        _controller.SetDestinationSpot(ApothecaryManager.Instance.alchemistSeat);
    }

    public override void UpdateState()
    {
        if (_controller.HasArrivedAtDestination())
        {

            _controller.ChangeAnimationTo(_controller.sitDownAnim);

            // Looks for a non assigned potion
            if (ApothecaryManager.Instance.currentAlchemistTurn < ApothecaryManager.Instance.generatedAlchemistTurns)
            {
                // Asignn a turn to the potion
                ApothecaryManager.Instance.NextAlchemistTurn();

                // Switches to pick up ingredients state
                SwitchState(_controller.pickingUpIngredientsState); 
            }
        }
    }

}
