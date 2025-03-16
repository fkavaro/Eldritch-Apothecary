using UnityEngine;

/// <summary>
/// Picks up a product or waits in line directly
/// </summary>
public class Shopping_ClientState : AClientState
{
    public Shopping_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext)
    {
        name = "Shopping";
    }

    public override void StartState()
    {
        // Set target to a random shop stand
        _clientContext.SetTarget(ApothecaryManager.Instance.RandomShopStand());
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
