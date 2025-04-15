using UnityEngine;

/// <summary>
/// Waits in line to be attended by the receptionist 
/// </summary>
public class WaitForReceptionist_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{
    public WaitForReceptionist_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Waiting in line", sfsm) { }

    public override void StartState()
    {
        // Add client to the queue
        //ApothecaryManager.Instance.EnterQueue(_behaviourController);

        // Set target to the last position in line
        _behaviourController.SetTargetPos(ApothecaryManager.Instance.waitingQueue.LastInLine());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Update state time
        _stateTime += Time.deltaTime;

        // Has been waiting for too long
        if (_behaviourController.maxMinutesWaiting <= _stateTime / 60f)
        {
            _stateMachine.SwitchState(_behaviourController.complainingState);
        }
        // Destination is the last position in line and is not in the queue
        else if (_behaviourController.HasArrived(ApothecaryManager.Instance.waitingQueue.LastInLine())
                && !ApothecaryManager.Instance.waitingQueue.Contains(_behaviourController))
        {
            // Enters queue
            ApothecaryManager.Instance.waitingQueue.Enter(_behaviourController);
        }
        // Destination is the receptionist counter, first position in line
        else if (_behaviourController.HasArrived(ApothecaryManager.Instance.waitingQueue.FirstInLine()))
        {
            // Talks before changing state
            _behaviourController.transform.LookAt(ApothecaryManager.Instance.receptionist.transform.position);
            _behaviourController.ChangeAnimationTo(_behaviourController.talkAnim);

            if (_behaviourController.wantedService == Client.WantedService.OnlyShop)
                _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, "Talking"));
            else
                _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.waitForServiceState, "Talking"));
        }
        // Destination is the next queue position
        else if (_behaviourController.HasArrived())
        {
            ApothecaryManager.Instance.waitingQueue.FixRotation(_behaviourController);
            _behaviourController.ChangeAnimationTo(_behaviourController.waitAnim);
        }

    }

    public override void ExitState()
    {
        // Leave the queue for next turn
        ApothecaryManager.Instance.waitingQueue.NextTurn();

        _stateTime = 0f;
    }
}
