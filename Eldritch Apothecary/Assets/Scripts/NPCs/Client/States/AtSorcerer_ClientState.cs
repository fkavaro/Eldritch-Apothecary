using UnityEngine;

/// <summary>
/// Attends the sorcerer until its service is finished
/// </summary>
public class AtSorcerer_ClientState : AClientState
{
    public AtSorcerer_ClientState(StackFiniteStateMachine stackFsm, Client client) : base(stackFsm, client) { }

    public override void StartState()
    {
        clientContext.SetTarget(ApothecaryManager.Instance.sorcererSeat.position);
    }

    public override void UpdateState()
    {
        if (coroutineStarted) return;

        // Has reached the sorcerer seat
        if (clientContext.HasArrived())
        {
            clientContext.ChangeAnimationTo(clientContext.sitDownAnim); // TODO: stand up animation
            clientContext.StartCoroutine(WaitAndSwitchState(clientContext.leavingState, "Sitting down"));
        }
    }
}