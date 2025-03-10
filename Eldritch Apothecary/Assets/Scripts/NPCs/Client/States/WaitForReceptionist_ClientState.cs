using UnityEngine;

/// <summary>
/// Waits in line to be attended by the receptionist 
/// </summary>
public class WaitForReceptionist_ClientState : AClientState
{
    public WaitForReceptionist_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext) { }

    public override void StartState()
    {
        // Add client to the queue
        ApothecaryManager.Instance.waitingQueue.Add(clientContext);
    }

    public override void UpdateState()
    {
        if (coroutineStarted) return;

        // Update state time
        _stateTime += Time.deltaTime;

        // If client has reached the receptionist counter, first position in line
        if (clientContext.HasArrived(ApothecaryManager.Instance.waitingQueue.FirstInLine()))
        {
            clientContext.ChangeAnimationTo(clientContext.talkAnim);
            clientContext.StartCoroutine(WaitAndSwitchState(clientContext.waitForServiceState, "Talking"));
        }
        // Has been waiting for too long
        else if (clientContext.maxMinutesWaiting <= _stateTime / 60f)
        {
            stackFsm.SwitchState(clientContext.complainingState);
        }
        // Has advanced in the queue and arrived to a new position
        else if (clientContext.HasArrived())
        {
            clientContext.ChangeAnimationTo(clientContext.waitAnim);
        }
    }

    public override void ExitState()
    {
        // Remove client from the queue
        ApothecaryManager.Instance.waitingQueue.Leave();

        _stateTime = 0f;
    }
}
