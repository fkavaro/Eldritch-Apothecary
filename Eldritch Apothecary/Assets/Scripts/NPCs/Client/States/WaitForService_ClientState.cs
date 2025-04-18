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
        _behaviourController.SetDestinationSpot(ApothecaryManager.Instance.RandomWaitingSeat());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached the waiting seat
        if (_behaviourController.HasArrivedAtDestination())
        {
            // It's its turn
            if (ApothecaryManager.Instance.IsTurn(_behaviourController))
            {
                switch (_behaviourController.wantedService)
                {
                    case Client.WantedService.Sorcerer:
                        SwitchState(_behaviourController.atSorcererState);
                        break;

                    case Client.WantedService.Alchemist:
                        _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.pickPotionUpState, _behaviourController.sitDownAnim, "Sitting down"));

                        SwitchState(_behaviourController.pickPotionUpState);
                        break;
                    default:
                        SwitchState(_behaviourController.leavingState);
                        break;
                }
            }
            else // It's not
                // Wait
                _behaviourController.ChangeAnimationTo(_behaviourController.sitDownAnim);
        }
    }
}
