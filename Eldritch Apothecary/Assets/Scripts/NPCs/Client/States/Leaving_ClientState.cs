using UnityEngine;

/// <summary>
/// Leaves the apothecary, returning to the pool.
/// </summary>
public class Leaving_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{
    public Leaving_ClientState(StackFiniteStateMachine<Client> sfsm) : base(sfsm)
    {
        stateName = "Leaving";
    }

    public override void StartState()
    {
        _behaviourController.SetTargetPos(ApothecaryManager.Instance.queueExitPosition.position);
    }

    public override void UpdateState()
    {
        // If client has reached the exit
        if (_behaviourController.HasArrived(ApothecaryManager.Instance.exitPosition.position))
            ApothecaryManager.Instance.clientsPool.Release(_behaviourController);
        else if (_behaviourController.HasArrived(ApothecaryManager.Instance.queueExitPosition.position))
            _behaviourController.SetTargetPos(ApothecaryManager.Instance.exitPosition.position);

    }
}