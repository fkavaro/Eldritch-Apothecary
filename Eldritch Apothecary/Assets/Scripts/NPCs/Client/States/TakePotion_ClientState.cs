using UnityEngine;

/// <summary>
/// Picks up its potion
/// </summary>
public class TakePotion_ClientState : ANPCState<Client, FiniteStateMachine<Client>>
{
    Potion assignedPotion;

    public TakePotion_ClientState(FiniteStateMachine<Client> fsm)
    : base("Picking potion", fsm) { }

    public override void StartState()
    {
        assignedPotion = ApothecaryManager.Instance.AssignedPotion(_controller);
        _controller.SetDestination(assignedPotion.transform.position);
    }

    public override void UpdateState()
    {
        // Is close to its potion
        if (_controller.IsCloseToDestination(1, true))
        {
            // Take it
            assignedPotion.Take();
            // Leave
            SwitchStateAfterCertainTime(1f, _controller.fsmAction.leavingState, _controller.pickUpAnim, "Picking up potion");
        }
    }
}