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
        _controller.SetDestinationSpot(ApothecaryManager.Instance.sorcererSeat);
    }

    public override void UpdateState()
    {
        // // Has reached the sorcerer seat
        // if (_behaviourController.HasArrivedAtDestination())
        //     _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, _behaviourController.sitDownAnim, "Sitting down"));

        // Is close to the sorcerer seat
        if (_controller.IsCloseToDestination())
        {
            // Sorcerer seat is occupied
            if (_controller.DestinationSpotIsOccupied())
            {
                // Stop and wait
                _controller.SetIfStopped(true);
                _controller.ChangeAnimationTo(_controller.waitAnim);
            }
            else // sorcerer seat is free
            {
                _controller.SetIfStopped(false);

                // Has reached exact position
                if (_controller.HasArrivedAtDestination())
                    _controller.StartCoroutine(SwitchStateAfterRandomTime(_controller.leavingState, _controller.sitDownAnim, "Sitting down"));
            }
        }
    }
}