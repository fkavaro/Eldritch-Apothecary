using BehaviourAPI.StateMachines.StackFSMs;
using UnityEngine;

/// <summary>
/// Picks up a product or waits in line directly
/// </summary>
public class Shopping_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public Shopping_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Shopping", sfsm) { }

    public override void StartState()
    {
        _controller.SetDestinationSpot(ApothecaryManager.Instance.RandomShopShelves());
    }

    public override void UpdateState()
    {
        // Is close to the shelves spot
        if (_controller.IsCloseToDestination())
        {
            // Shelves spot is occupied
            if (_controller.DestinationSpotIsOccupied())
            {
                // Stop and wait
                _controller.SetIfStopped(true);
                _controller.ChangeAnimationTo(_controller.waitAnim);
            }
            else // Spot is free
            {
                _controller.SetIfStopped(false);

                // Has reached exact position
                if (_controller.HasArrivedAtDestination())
                    // Pick up a product and change to the next state
                    _controller.StartCoroutine(SwitchStateAfterRandomTime(_controller.waitForReceptionistState, _controller.pickUpAnim, "Picking up objects"));
            }
        }
    }
}
