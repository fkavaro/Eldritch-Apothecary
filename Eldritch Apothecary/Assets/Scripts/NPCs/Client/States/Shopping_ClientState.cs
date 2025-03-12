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
        // Switch state if client wants any other service: sorcerer or alchemist
        if (_clientContext.wantedService != Client.WantedService.OnlyShop)
            _stackFsm.SwitchState(_clientContext.waitForReceptionistState);
        else
            _clientContext.SetTarget(ApothecaryManager.Instance.RandomShopStand());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        if (_clientContext.HasArrived())
        {
            _clientContext.ChangeAnimationTo(_clientContext.pickUpAnim);
            _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.waitForReceptionistState, "Picking up objects"));
        }
    }

    public override void ExitState()
    {
        //clientContext.StopAgent();
    }
}
