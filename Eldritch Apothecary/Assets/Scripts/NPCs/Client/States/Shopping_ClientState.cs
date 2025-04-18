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
        // Shelves spot is the target spot
        _behaviourController.SetDestinationSpot(ApothecaryManager.Instance.RandomShopShelves());

    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Is close to the shelves spot
        if (_behaviourController.IsCloseToDestination())
        {
            // Shelves spot is occupied
            if (_behaviourController.DestinationSpotIsOccupied())
            {
                // Wait
                _behaviourController.ChangeAnimationTo(_behaviourController.waitAnim);
            }
            else // Spot is free
            {
                // Has reached exact position
                if (_behaviourController.HasArrivedAtDestination())
                    // Pick up a product and change to the next state
                    _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.waitForReceptionistState, _behaviourController.pickUpAnim, "Picking up objects"));
            }
        }
    }
}
