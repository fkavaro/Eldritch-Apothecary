using UnityEngine;

/// <summary>
/// Is startled by the grumpy cat. Can lead to a complain.
/// </summary>
public class Stunned_ClientState : AClientState
{

    public Stunned_ClientState(StackFiniteStateMachine fsm, Client clientContext) : base(fsm, clientContext) { }

    public override void StartState()
    {
        clientContext.StopAgent();

        clientContext.ChangeAnimationTo(clientContext.stunnedAnim);

        clientContext.StartCoroutine(WaitAndSwitchState(3f, clientContext.complainingState, "Stunned")); // TODO: Just wait until animation is executed

        if (clientContext.HasReachedMaxScares())
            stackFsm.SwitchState(clientContext.complainingState);
        else
            stackFsm.ReturnToPreviousState();

    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        clientContext.ReactivateAgent();
    }
}
