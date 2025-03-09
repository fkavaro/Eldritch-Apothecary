using UnityEngine;

public class Complaining_ClientState : AClientState
{
    public Complaining_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext) { }

    public override void StartState()
    {
        clientContext.SetTarget(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateState()
    {
        if (coroutineStarted) return;

        // Has reached the complaining position
        if (clientContext.HasArrived())
        {
            clientContext.ChangeAnimationTo(clientContext.complainAnim);
            clientContext.StartCoroutine(WaitAndSwitchState(clientContext.leavingState, "Complaining"));
        }
    }

    public override void ExitState()
    {

    }
}
