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
            _controller.SetIfStopped(false);
            SwitchStateAfterRandomTime(_controller.leavingState, _controller.sitDownAnim, "Sitting down");
        }
        // Is close to the sorcerer seat but it's occupied
        else if (_controller.IsCloseToDestination() && _controller.DestinationSpotIsOccupied())
        {
            // Stop and wait
            _controller.SetIfStopped(true);
            _controller.ChangeAnimationTo(_controller.waitAnim);
        }
    }
}