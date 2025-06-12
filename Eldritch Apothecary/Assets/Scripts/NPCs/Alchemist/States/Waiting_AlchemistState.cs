using UnityEngine;

public class Waiting_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public Waiting_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Waiting", stackFsm) { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        // Go to alchemist Table
        _controller.SetDestinationSpot(ApothecaryManager.Instance.alchemistSeat);
    }

    public override void UpdateState()
    {
        if (_controller.HasArrivedAtDestination())
        {

            _controller.ChangeAnimationTo(_controller.sitDownAnim);

            // Look for non assigned potion
            if (ApothecaryManager.Instance.currentAlchemistTurn < ApothecaryManager.Instance.generatedAlchemistTurns)
            {
                // Asignar turno a esa poción
                ApothecaryManager.Instance.NextAlchemistTurn();

                // Next State
                SwitchState(_controller.pickingUpIngredientsState); 
            }
        }
    }

}
