using UnityEngine;

/// <summary>
/// Waits in line to be attended by the receptionist 
/// </summary>
public class WaitForReceptionist_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public WaitForReceptionist_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Waiting in line", sfsm) { }

    public override void StartState()
    {
        // Set target to the last position in line
        _behaviourController.SetDestination(ApothecaryManager.Instance.waitingQueue.LastInLinePos());
    }

    public override void UpdateState()
    {
        // Update state time
        _stateTime += Time.deltaTime;

        if (_coroutineStarted) return;

        // Has been waiting first in line for too long
        if (_behaviourController.FirstInLineTooLong())
        {
            SwitchState(_behaviourController.complainingState);
        }
        // Is close to the last position in line and is not in the queue
        else if (_behaviourController.IsCloseTo(ApothecaryManager.Instance.waitingQueue.LastInLinePos())
                && !_behaviourController.InWaitingQueue())
        {
            // Enters queue
            _behaviourController.EnterWaitingQueue();
        }
        // Has arrived the receptionist counter, first position in line
        else if (_behaviourController.HasArrived(ApothecaryManager.Instance.waitingQueue.FirstInLinePos()))
        {
            // Can interact with receptionist: is ready to attend clients at the counter
            if (ApothecaryManager.Instance.receptionist.CanAttend())
            {
                // Talks before changing state
                _behaviourController.transform.LookAt(ApothecaryManager.Instance.receptionist.transform.position);

                if (_behaviourController.wantedService == Client.WantedService.OnlyShop)
                    _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, _behaviourController.talkAnim, "Talking"));
                else
                    _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.waitForServiceState, _behaviourController.talkAnim, "Talking"));
            }
            // Receptionist is not at the counter
            else
            {
                // Increase time waiting
                _behaviourController.timeWaiting += Time.deltaTime;
                // Normalize time between 0 and the 1 as the maximum waiting time of the client
                _behaviourController.normalizedWaitingTime = Mathf.Clamp01(_behaviourController.timeWaiting / _behaviourController.maxMinutesWaiting * 60f);
            }
        }
        // Has arrived the next queue position
        else if (_behaviourController.HasArrivedAtDestination())
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
        _behaviourController.timeWaiting = 0f;
        _behaviourController.normalizedWaitingTime = 0f;
    }
}
