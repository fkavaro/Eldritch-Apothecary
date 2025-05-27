using UnityEngine;

/// <summary>
/// Picks up its potion
/// </summary>
public class PickPotionUp_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public PickPotionUp_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Picking potion", sfsm) { }

    public override void StartState()
    {
        _controller.SetDestination(ApothecaryManager.Instance.RandomPickUp());
    }

    public override void UpdateState()
    {
        // Is close to the pick up position
        if (_controller.IsCloseToDestination())
        {
            // Pick up position is occupied
            if (_controller.DestinationSpotIsOccupied())
            {
                // Stop and wait
                _controller.SetIfStopped(true);
                _controller.ChangeAnimationTo(_controller.waitAnim, true);
            }
            else // Pick up position is free
            {
                _controller.SetIfStopped(false);

                // Has reached exact position
                if (_controller.HasArrivedAtDestination())
                    SwitchStateAfterCertainTime(1f, _controller.leavingState, _controller.pickUpAnim, "Picking up potion", true);
            }
        }
    }
}