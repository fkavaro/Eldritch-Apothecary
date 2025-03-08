using UnityEngine;

public class Stunned_ClientState : AClientState
{

    public Stunned_ClientState(StackFiniteStateMachine fsm, Client clientContext) : base(fsm, clientContext) { }

    public override void StartState()
    {
        clientContext.StopAgent();

        // Stunned animation
        clientContext.ChangeAnimationTo(clientContext.Talk); // TODO: Change to stunned animation
    }

    public override void UpdateState()
    {
        clientContext.Wait(3f); // Wait for animation to play

        // If client has been scared enough times
        if (clientContext.HasReachedMaxScares())
        {
            // Switch to complaining state
            stackFsm.SwitchState(clientContext.complainingState);
        }
        else
        {
            // Return to previous state
            stackFsm.ReturnToPreviousState();
        }
    }

    public override void ExitState()
    {
        clientContext.ReactivateAgent();
    }
}
