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
        _clientContext.SetTarget(ApothecaryManager.Instance.exitPosition.position);
    }

    public override void UpdateState()
    {
        // If client has reached the exit
        if (_clientContext.HasArrived(1f))
            ApothecaryManager.Instance.clientsPool.Release(_clientContext);
    }
}