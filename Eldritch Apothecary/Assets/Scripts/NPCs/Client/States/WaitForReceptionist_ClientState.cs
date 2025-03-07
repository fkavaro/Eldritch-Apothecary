using UnityEngine;

public class WaitForReceptionist_ClientState : AClientState
{
    public WaitForReceptionist_ClientState(FiniteStateMachine fsm, Client clientController) : base(fsm, clientController) { }

    public override void StartState()
    {
        // Add client to the queue
        ApothecaryManager.Instance.AddToQueue(clientController);
    }

    public override void UpdateState()
    {
        // If client has reached the receptionist counter, first position in line
        if (clientController.HasArrived(ApothecaryManager.Instance.queuePositions[0].position))
        {
            // Talk animation

            // Switch state to talking to receptionist
            fsm.SwitchState(clientController.shopping);
        }
    }

    public override void ExitState()
    {
        // Remove client from the queue
        ApothecaryManager.Instance.DeQueue(clientController);
    }
}
