using UnityEngine;

/// <summary>
/// Waits for its service turn sat down.
/// </summary>
public class WaitForService_ClientState : AClientState
{
    public WaitForService_ClientState(StackFiniteStateMachine stackFsm, Client client) : base(stackFsm, client) { }

    public override void StartState()
    {
        clientContext.SetTarget(ApothecaryManager.Instance.RandomSeat());
    }

    public override void UpdateState()
    {
        if (coroutineStarted) return;

        // Has reached the waiting seat
        if (clientContext.HasArrived())
        {
            clientContext.ChangeAnimationTo(clientContext.sitDownAnim); // TODO: stand up animation

            switch (clientContext.wantedService)
            {
                case Client.WantedService.Sorcerer:
                    clientContext.StartCoroutine(WaitAndSwitchState(clientContext.atSorcererState, "Sitting down"));
                    break;
                case Client.WantedService.Alchemist:
                    clientContext.StartCoroutine(WaitAndSwitchState(clientContext.pickPotionUpState, "Sitting down"));
                    break;
                default:
                    clientContext.StartCoroutine(WaitAndSwitchState(clientContext.leavingState, "Sitting down"));
                    break;
            }

        }
        // If service is ready
        // Stand up animation
        // Switch state according to service
    }
}
