using UnityEngine;

public class WaitForReceptionist_ClientState : AClientState
{
    float _secondsWaiting = 0f;
    public WaitForReceptionist_ClientState(StackFiniteStateMachine stackFsm, Client clientController) : base(stackFsm, clientController) { }

    public override void StartState()
    {
        // Add client to the queue
        ApothecaryManager.Instance.AddToQueue(clientController);
    }

    public override void UpdateState()
    {
        _secondsWaiting += Time.deltaTime;

        // If client has reached the receptionist counter, first position in line
        if (clientController.HasArrived(ApothecaryManager.Instance.queuePositions[0].position))
        {
            // Talk animation

            // Switch state to waiting for service
            stackFsm.SwitchState(clientController.shoppingState); // TODO: Change to waiting for service
        }
        // Has been waiting for too long
        else if (clientController.maxMinutesWaiting <= _secondsWaiting / 60f)
        {
            // Switch to complaining state
            stackFsm.SwitchState(clientController.complainingState);
        }
    }

    public override void ExitState()
    {
        // Remove client from the queue
        ApothecaryManager.Instance.DeQueue(clientController);
    }
}
