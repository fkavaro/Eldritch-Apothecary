using BehaviourAPI.StateMachines.StackFSMs;
using UnityEngine;

/// <summary>
/// Picks up a product or waits in line directly
/// </summary>
public class Shopping_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{
    Spot shelves;

    public Shopping_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Shopping", sfsm) { }

    public override void StartState()
    {
        shelves = ApothecaryManager.Instance.RandomShopShelves();
        _behaviourController.SetTargetSpot(shelves);

    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // // Got close to the shelves
        // if (_behaviourController.HasArrived(2f, false))
        // {
        //     // Shelves spot is occupied
        //     if (shelves.IsOccupied())
        //     {
        //         // Wait
        //         _behaviourController.ChangeAnimationTo(_behaviourController.waitAnim);
        //     }
        //     else // Spot is free
        //     {
        // Has reached exact position
        if (_behaviourController.HasArrived())
        {
            _behaviourController.ChangeAnimationTo(_behaviourController.pickUpAnim);
            _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.waitForReceptionistState, "Picking up objects"));
        }
        //}
        //}
    }
}
