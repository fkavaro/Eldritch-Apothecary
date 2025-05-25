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
        _controller.SetDestination(ApothecaryManager.Instance.waitingQueue.LastInLinePos());
    }

    public override void UpdateState()
    {
        // Has been waiting first in line for too long
        // if (_controller.WaitedTooLong())
        // {
        //     SwitchState(_controller.complainingState);
        // }

        // Is close to the last position in line and is not in the queue
        if (_controller.IsCloseTo(ApothecaryManager.Instance.waitingQueue.LastInLinePos())
                && !_controller.InWaitingQueue())
        {
            // Reduce avoidance radius to avoid being blocked by other clients
            _controller.SetAvoidanceRadius(0.5f);
            // Enters queue
            _controller.EnterWaitingQueue();
        }
        // Has arrived the receptionist counter, first position in line
        else if (_controller.HasArrived(ApothecaryManager.Instance.waitingQueue.FirstInLinePos()))
        {
            _controller.transform.LookAt(ApothecaryManager.Instance.receptionist.transform.position);

            // Can interact with receptionist: is ready to attend clients at the counter
            if (ApothecaryManager.Instance.receptionist.CanAttend())
            {
                if (_controller.wantedService == Client.WantedService.OnlyShop)
                    SwitchStateAfterRandomTime(_controller.leavingState, _controller.talkAnim, "Talking");
                else
                    SwitchStateAfterRandomTime(_controller.waitForServiceState, _controller.talkAnim, "Talking");
            }
            // Receptionist is not at the counter
            else
            {
                // Increase time waiting
                _controller.secondsWaitingFirstInLine += Time.deltaTime;
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
        // Leave the queue for next turn
        ApothecaryManager.Instance.waitingQueue.NextTurn();

        _controller.ResetAvoidanceRadius(); // Reset avoidance radius

        _controller.secondsWaitingFirstInLine = 0f;
        _controller.normalizedWaitingTime = 0f;
    }
}
