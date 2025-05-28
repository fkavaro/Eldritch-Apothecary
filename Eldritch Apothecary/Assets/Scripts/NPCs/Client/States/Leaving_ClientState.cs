using UnityEngine;

/// <summary>
/// Leaves the apothecary, returning to the pool.
/// </summary>
public class Leaving_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    Potion assignedPotion;
    Vector3 queueExitPosition;

    public Leaving_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Leaving", sfsm) { }

    public override void StartState()
    {
        _controller.DontMindAnything(); // So that it can't be stunned or complain again

        // Has a potion assigned
        if (_controller.wantedService == Client.WantedService.POTION)
            assignedPotion = ApothecaryManager.Instance.AssignedPotion(_controller);

        if (assignedPotion)
        {
            _controller.SetDestination(assignedPotion.transform.position);
        }
        else
        {
            _controller.turnText.text = "";
            queueExitPosition = ApothecaryManager.Instance.queueExitPosition.position;
            _controller.SetDestination(queueExitPosition);
        }
    }

    public override void UpdateState()
    {
        // Is close to the queue exit
        if (_controller.IsCloseTo(queueExitPosition, 3f))
            _controller.SetDestination(ApothecaryManager.Instance.exitPosition.position);
        // Is close to the apothecary exit
        else if (_controller.IsCloseTo(ApothecaryManager.Instance.exitPosition.position))
            ApothecaryManager.Instance.clientsPool.Release(_controller);
        // Is close to its potion
        else if (assignedPotion && _controller.IsCloseTo(assignedPotion.transform.position, 1, true))
        {
            // Take it
            _controller.PlayAnimationCertainTime(1f, _controller.pickUpAnim, "Taking potion", () => { }, false);
            assignedPotion.Take();
            _controller.turnText.text = "";
        }
    }
}