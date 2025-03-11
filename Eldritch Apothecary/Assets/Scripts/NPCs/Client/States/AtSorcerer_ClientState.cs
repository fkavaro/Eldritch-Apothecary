using UnityEngine;

/// <summary>
/// Attends the sorcerer until its service is finished
/// </summary>
public class AtSorcerer_ClientState : AClientState
{
    public AtSorcerer_ClientState(StackFiniteStateMachine stackFsm, Client client) : base(stackFsm, client) { }

    public override void StartState()
    {
        _clientContext.SetTarget(ApothecaryManager.Instance.sorcererSeat.position);
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached the sorcerer seat
        if (_clientContext.HasArrived())
        {
            _clientContext.ChangeAnimationTo(_clientContext.sitDownAnim); // TODO: stand up animation
            _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.leavingState, "Sitting down"));
        }
    }
}