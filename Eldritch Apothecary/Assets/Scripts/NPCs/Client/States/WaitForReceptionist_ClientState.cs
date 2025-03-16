using UnityEngine;

/// <summary>
/// Waits in line to be attended by the receptionist 
/// </summary>
public class WaitForReceptionist_ClientState : AClientState
{
    public WaitForReceptionist_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext)
    {
        name = "Wait for receptionist";
    }

    public override void StartState()
    {
        // Add client to the queue
        ApothecaryManager.Instance.waitingQueue.Add(_clientContext);
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Update state time
        _stateTime += Time.deltaTime;

        // Has been waiting for too long
        if (_clientContext.maxMinutesWaiting <= _stateTime / 60f)
        {
            _stackFsm.SwitchState(_clientContext.complainingState);
        }
        // Has reached the receptionist counter, first position in line
        else if (_clientContext.HasArrived(ApothecaryManager.Instance.waitingQueue.FirstInLine()))
        {
            _clientContext.ChangeAnimationTo(_clientContext.talkAnim);
            _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.waitForServiceState, "Talking"));
        }
        // Has advanced in the queue and arrived to a new position
        else if (_clientContext.HasArrived())
        {
            _clientContext.ChangeAnimationTo(_clientContext.waitAnim);
        }
    }

    public override void ExitState()
    {
        // Remove client from the queue
        ApothecaryManager.Instance.waitingQueue.Leave();

        _stateTime = 0f;
    }
}
