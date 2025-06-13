using UnityEngine;
using static Client;

/// <summary>
/// Waits in line to be attended by the receptionist 
/// </summary>
public class WaitForReceptionist_ClientState : ANPCState<Client, FiniteStateMachine<Client>>
{
    public WaitForReceptionist_ClientState(FiniteStateMachine<Client> fsm)
    : base("Waiting in line", fsm) { }

    public override void StartState()
    {
        _controller.ResetWaitingTime();

        // Set target to the last position in line
        _controller.SetDestination(ApothecaryManager.Instance.waitingQueue.LastInLinePos());
    }

    public override void UpdateState()
    {
        // Is close to the last position in line and is not in the queue
        if (_controller.IsCloseTo(ApothecaryManager.Instance.waitingQueue.LastInLinePos()))
        {
            // Reduce avoidance radius to avoid being blocked by other clients
            _controller.SetAvoidanceRadius(0.5f);
            // Enters queue (if not already)
            ApothecaryManager.Instance.waitingQueue.Enter(_controller);
        }
        // Has arrived the receptionist counter, first position in line
        else if (_controller.HasArrived(ApothecaryManager.Instance.waitingQueue.FirstInLinePos()))
        {
            _controller.transform.LookAt(ApothecaryManager.Instance.receptionist.transform.position);

            // Can interact with receptionist: is ready to attend clients at the counter
            if (ApothecaryManager.Instance.receptionist.canAttend)
            {
                if (_controller.wantedService == WantedService.SHOPPING)
                    SwitchStateAfterRandomTime(_controller.fsmAction.leavingState, _controller.talkAnim, "Talking");
                else
                    SwitchStateAfterRandomTime(_controller.fsmAction.waitForServiceState, _controller.talkAnim, "Talking");
            }
            // Receptionist is not at the counter
            else
            {
                _controller.ChangeAnimationTo(_controller.waitAnim);
                // Increase time waiting
                _controller.secondsWaiting += Time.deltaTime;
            }
        }
        // Has arrived the next queue position
        else if (_controller.HasArrivedAtDestination())
        {
            ApothecaryManager.Instance.waitingQueue.FixRotation(_controller);
            _controller.ChangeAnimationTo(_controller.waitAnim);
        }
    }

    public override void ExitState()
    {
        // Still in waiting queue 
        if (ApothecaryManager.Instance.waitingQueue.Contains(_controller))
            // Leave the queue for next turn
            ApothecaryManager.Instance.waitingQueue.NextTurn();

        _controller.ResetAvoidanceRadius();
        _controller.ResetWaitingTime();
    }
}
