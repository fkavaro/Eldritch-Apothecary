using UnityEngine;

/// <summary>
/// Waits in line to be attended by the receptionist 
/// </summary>
public class WaitForReceptionist_ClientState : AClientState
{
    public WaitForReceptionist_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext)
    {
        stateName = "Wait for receptionist";
    }

    public override void StartState()
    {
        // Add client to the queue
        //ApothecaryManager.Instance.EnterQueue(_clientContext);

        // Set target to the last position in line
        _clientContext.SetTarget(ApothecaryManager.Instance.waitingQueue.LastInLine());
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
        // Destination is the last position in line and is not in the queue
        else if (_clientContext.HasArrived(ApothecaryManager.Instance.waitingQueue.LastInLine())
                && !ApothecaryManager.Instance.waitingQueue.Contains(_clientContext))
        {
            // Enters queue
            ApothecaryManager.Instance.waitingQueue.Enter(_clientContext);
        }
        // Destination is the receptionist counter, first position in line
        else if (_clientContext.HasArrived(ApothecaryManager.Instance.waitingQueue.FirstInLine()))
        {
            // Talks before changing state
            _clientContext.transform.LookAt(ApothecaryManager.Instance.receptionist.transform.position);
            _clientContext.ChangeAnimationTo(_clientContext.talkAnim);

            if (_clientContext.wantedService == Client.WantedService.OnlyShop)
                _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.leavingState, "Talking"));
            else
                _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.waitForServiceState, "Talking"));
        }
        // Destination is the next queue position
        else if (_clientContext.HasArrived())
        {
            ApothecaryManager.Instance.waitingQueue.FixRotation(_clientContext);
            _clientContext.ChangeAnimationTo(_clientContext.waitAnim);
        }

    }

    public override void ExitState()
    {
        // Leave the queue for next turn
        ApothecaryManager.Instance.waitingQueue.NextTurn();

        _stateTime = 0f;
    }
}
