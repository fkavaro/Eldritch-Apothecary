using UnityEngine;

public class Stunned_ClientState : AClientState
{

    public Stunned_ClientState(StackFiniteStateMachine fsm, Client clientContext) : base(fsm, clientContext) { }

    public override void StartState()
    {
        clientContext.StopAgent();

        clientContext.ChangeAnimationTo(clientContext.Stunned);

        clientContext.StartCoroutine(WaitAndSwitchState(3f, clientContext.complainingState));

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
