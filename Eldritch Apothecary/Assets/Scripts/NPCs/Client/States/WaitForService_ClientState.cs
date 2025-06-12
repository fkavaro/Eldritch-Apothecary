using UnityEngine;

/// <summary>
/// Waits for its service turn sat down.
/// </summary>
public class WaitForService_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public WaitForService_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Waiting for service", sfsm) { }

    public override void StartState()
    {
        ApothecaryManager.Instance.TakeTurn(_controller);
        _controller.SetDestinationSpot(ApothecaryManager.Instance.RandomWaitingSeat());
    }

    public override void UpdateState()
    {

        // Is close to the waiting seat
        if (_controller.IsCloseToDestination())
        {
            // It's its turn
            if (ApothecaryManager.Instance.IsTurn(_controller))
            {
                switch (_controller.wantedService)
                {
                    case Client.WantedService.SPELL:
                        SwitchState(_controller.fsmAction.atSorcererState);
                        break;

                    case Client.WantedService.POTION:
                        SwitchState(_controller.fsmAction.pickPotionUpState);
                        break;

                    default:
                        SwitchState(_controller.fsmAction.leavingState);
                        break;
                }
            }
            // It's not
            else
            {
                // Has reached the waiting seat
                if (_controller.HasArrivedAtDestination())
                {
                    // Increase time waiting
                    _controller.secondsWaiting += Time.deltaTime;
                    // Wait
                    _controller.ChangeAnimationTo(_controller.sitDownAnim);
                }
            }
        }
    }
}
