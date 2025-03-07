using UnityEngine;

public class Stunned_ClientState : AClientState
{

    public Stunned_ClientState(StackFiniteStateMachine fsm, Client clientController) : base(fsm, clientController) { }

    public override void StartState()
    {
        clientController.StopAgent();

        // Stunned animation
    }

    public override void UpdateState()
    {
        // If client has been scared enough times
        if (clientController.HasReachedMaxScares())
        {
            // Switch to complaining state
            stackFsm.SwitchState(clientController.complainingState);
        }
        else
        {
            // Return to previous state
            stackFsm.ReturnToPreviousState();
        }
    }

    public override void ExitState()
    {
        clientController.ReactivateAgent();
    }
}
