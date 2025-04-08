using UnityEngine;

/// <summary>
/// Leaves the apothecary, returning to the pool.
/// </summary>
public class Leaving_ClientState : AClientState
{
    public Leaving_ClientState(StackFiniteStateMachine stackFsm, Client client) : base(stackFsm, client)
    {
        stateName = "Leaving";
    }

    public override void StartState()
    {
        _clientContext.SetTarget(ApothecaryManager.Instance.queueExitPosition.position);
    }

    public override void UpdateState()
    {
        // If client has reached the exit
        if (_clientContext.HasArrived(ApothecaryManager.Instance.exitPosition.position))
            ApothecaryManager.Instance.clientsPool.Release(_clientContext);
        else if (_clientContext.HasArrived(ApothecaryManager.Instance.queueExitPosition.position))
            _clientContext.SetTarget(ApothecaryManager.Instance.exitPosition.position);

    }
}