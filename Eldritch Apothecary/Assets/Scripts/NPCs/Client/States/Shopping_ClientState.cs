using BehaviourAPI.StateMachines.StackFSMs;
using UnityEngine;

/// <summary>
/// Picks up a product or waits in line directly
/// </summary>
public class Shopping_ClientState : AClientState
{
    Vector3 stand;

    public Shopping_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext)
    {
        stateName = "Shopping";
    }

    public override void StartState()
    {
        // Try to get a random shop stand
        stand = ApothecaryManager.Instance.shop.RandomStand(_clientContext);

        // Leave if no available stands
        if (stand == Vector3.zero)
            _stackFsm.SwitchState(_clientContext.leavingState);
        // Go to available stand
        else
            _clientContext.SetTarget(stand);
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Arrived at shop stand
        if (_clientContext.HasArrived())
        {
            _clientContext.ChangeAnimationTo(_clientContext.pickUpAnim);
            _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.waitForReceptionistState, "Picking up objects"));
        }
    }

    public override void ExitState()
    {

    }
}
