using UnityEngine;

/// <summary>
/// Is startled by the grumpy cat. Can lead to a complain.
/// </summary>
public class Stunned_ClientState : AClientState
{

    public Stunned_ClientState(StackFiniteStateMachine fsm, Client clientContext) : base(fsm, clientContext)
    {
        name = "Stunned";
    }

    public override void StartState()
    {
        _clientContext.StopAgent();

        _clientContext.ChangeAnimationTo(_clientContext.stunnedAnim);

        _clientContext.StartCoroutine(WaitAndSwitchState(3f, _clientContext.complainingState, "Stunned")); // TODO: Just wait until animation is executed

        if (_clientContext.HasReachedMaxScares())
            _stackFsm.SwitchState(_clientContext.complainingState);
        else
            _stackFsm.ReturnToPreviousState();

    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        _clientContext.ReactivateAgent();
    }
}
