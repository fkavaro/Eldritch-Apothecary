using UnityEngine;

public class Complaining_ClientState : AClientState
{
    public Complaining_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext)
    {
        name = "Complaining";
    }

    public override void StartState()
    {
        _clientContext.SetTarget(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached the complaining position
        if (_clientContext.HasArrived())
        {
            _clientContext.ChangeAnimationTo(_clientContext.complainAnim);
            _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.leavingState, "Complaining"));
        }
    }

    public override void ExitState()
    {

    }
}
