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
        // Has reached the waiting seat
        if (_controller.HasArrivedAtDestination())
        {
            // It's its turn
            if (ApothecaryManager.Instance.IsTurn(_controller))
            {
                _controller.turnText.text = "";
                switch (_controller.wantedService)
                {
                    case Client.WantedService.SPELL:
                        SwitchState(_controller.atSorcererState);
                        break;

                    case Client.WantedService.POTION:
                        SwitchState(_controller.pickPotionUpState);
                        break;

                    default:
                        SwitchState(_controller.leavingState);
                        break;
                }
            }
            else // It's not
                // Wait
                _controller.ChangeAnimationTo(_controller.sitDownAnim);
        }
    }
}
