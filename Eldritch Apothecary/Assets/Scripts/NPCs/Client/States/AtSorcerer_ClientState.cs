using UnityEngine;

/// <summary>
/// Attends the sorcerer until its service is finished
/// </summary>
public class AtSorcerer_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public AtSorcerer_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("At sorcerer", sfsm) { }

    public override void StartState()
    {
        _controller.SetDestinationSpot(ApothecaryManager.Instance.clientSeat);
    }

    public override void UpdateState()
    {
        // Has reached exact position
        if (_controller.HasArrivedAtDestination())
        {
            SwitchStateAfterRandomTime(_controller.leavingState, _controller.sitDownAnim, "Sitting down", true);
        }
        // Is close to the sorcerer seat
        else if (_controller.IsCloseToDestination(4f))
        {
            // Is occupied
            if (_controller.DestinationSpotIsOccupied())
                // Stop and wait
                _controller.ChangeAnimationTo(_controller.waitAnim, true);
            else // Is free
                _controller.ChangeAnimationTo(_controller.walkAnim);
        }
    }
}