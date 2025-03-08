using UnityEngine;

public class WaitForReceptionist_ClientState : AClientState
{
    public WaitForReceptionist_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext) { }

    public override void StartState()
    {
        // Add client to the queue
        ApothecaryManager.Instance.AddToQueue(clientContext);
    }

    public override void UpdateState()
    {
        // Update state time
        _stateTime += Time.deltaTime;

        // If client has reached the receptionist counter, first position in line
        if (clientContext.HasArrived(ApothecaryManager.Instance.queuePositions[0].position))
        {
            clientContext.ChangeAnimationTo(clientContext.Talking);
            clientContext.StartCoroutine(WaitAndSwitchState(clientContext.shoppingState)); // TODO: Change to waiting for service
        }
        // Has been waiting for too long
        else if (clientContext.maxMinutesWaiting <= _stateTime / 60f)
        {
            stackFsm.SwitchState(clientContext.complainingState);
        }
        // Has advanced in the queue and arrived to a new position
        else if (clientContext.HasArrived())
        {
            clientContext.ChangeAnimationTo(clientContext.Idle);
        }
    }

    public override void ExitState()
    {
        // Remove client from the queue
        ApothecaryManager.Instance.DeQueue(clientContext);

        _stateTime = 0f;
    }
}
