using UnityEngine;

/// <summary>
/// Leaves the apothecary, returning to the pool.
/// </summary>
public class Leaving_ClientState : AClientState
{
    public Leaving_ClientState(StackFiniteStateMachine stackFsm, Client client) : base(stackFsm, client) { }

    public override void StartState()
    {
        clientContext.SetTarget(ApothecaryManager.Instance.exitPosition.position);
    }

    public override void UpdateState()
    {
        // If client has reached the exit
        if (clientContext.HasArrived(ApothecaryManager.Instance.exitPosition.position))
        {
            // Destroy gameobject
            GameObject.Destroy(clientContext.gameObject); // TODO: Return to clients pool
        }
    }
}