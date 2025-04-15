using UnityEngine;

/// <summary>
/// Waits for its service turn sat down.
/// </summary>
public class WaitForService_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{
    public WaitForService_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Waiting for service", sfsm) { }

    public override void StartState()
    {
        _behaviourController.SetTargetSpot(ApothecaryManager.Instance.RandomWaitingSeat());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached the waiting seat
        if (_behaviourController.HasArrived())
        {
            _behaviourController.ChangeAnimationTo(_behaviourController.sitDownAnim);

            switch (_behaviourController.wantedService)
            {
                case Client.WantedService.Sorcerer:
                    _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.atSorcererState, "Sitting down"));
                    break;
                case Client.WantedService.Alchemist:
                    _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.pickPotionUpState, "Sitting down"));
                    break;
                default:
                    _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, "Sitting down"));
                    break;
            }

        }
        // If service is ready
        // Stand up animation
        // Switch state according to service
    }
}
