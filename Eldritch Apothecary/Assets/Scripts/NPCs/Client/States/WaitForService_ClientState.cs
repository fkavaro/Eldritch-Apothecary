using UnityEngine;

/// <summary>
/// Waits for its service turn sat down.
/// </summary>
public class WaitForService_ClientState : AClientState
{
    public WaitForService_ClientState(StackFiniteStateMachine stackFsm, Client client) : base(stackFsm, client)
    {
        stateName = "Waiting for service";
    }

    public override void StartState()
    {
        _clientContext.SetTarget(ApothecaryManager.Instance.RandomWaitingSeat());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached the waiting seat
        if (_clientContext.HasArrived())
        {
            _clientContext.ChangeAnimationTo(_clientContext.sitDownAnim); // TODO: stand up animation

            switch (_clientContext.wantedService)
            {
                case Client.WantedService.Sorcerer:
                    _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.atSorcererState, "Sitting down"));
                    break;
                case Client.WantedService.Alchemist:
                    _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.pickPotionUpState, "Sitting down"));
                    break;
                default:
                    _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.leavingState, "Sitting down"));
                    break;
            }

        }
        // If service is ready
        // Stand up animation
        // Switch state according to service
    }
}
