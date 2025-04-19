using UnityEngine;

/// <summary>
/// Leaves the apothecary, returning to the pool.
/// </summary>
public class Leaving_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public Leaving_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Leaving", sfsm) { }

    public override void StartState()
    {
        Debug.Log("Starting leaving state");
        _controller.DontMindAnything(); // So that it can't be stunned or complain again
        _controller.SetDestination(ApothecaryManager.Instance.queueExitPosition.position);
    }

    public override void UpdateState()
    {
        Debug.Log("Updating leaving state");
        // Is close to the queue exit
        if (_controller.IsCloseTo(ApothecaryManager.Instance.queueExitPosition.position, 3f))
            _controller.SetDestination(ApothecaryManager.Instance.exitPosition.position);
        // Has reached the apothecary exit
        else if (_controller.HasArrived(ApothecaryManager.Instance.exitPosition.position))
            ApothecaryManager.Instance.clientsPool.Release(_controller);
    }
}