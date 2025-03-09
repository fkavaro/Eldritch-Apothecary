using UnityEngine;

/// <summary>
/// Picks up a product or waits in line directly
/// </summary>
public class Shopping_ClientState : AClientState
{
    public Shopping_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext) { }

    public override void StartState()
    {
        // Switch state if client wants any other service: sorcerer or alchemist
        if (clientContext.wantedService != Client.WantedService.OnlyShop)
            stackFsm.SwitchState(clientContext.waitForReceptionistState);
        else
            clientContext.SetTarget(ApothecaryManager.Instance.RandomShopStand());
    }

    public override void UpdateState()
    {
        if (coroutineStarted) return;

        if (clientContext.HasArrived())
        {
            clientContext.ChangeAnimationTo(clientContext.pickUpAnim);
            clientContext.StartCoroutine(WaitAndSwitchState(clientContext.waitForReceptionistState, "Picking up objects"));
        }
    }

    public override void ExitState()
    {
        //clientContext.StopAgent();
    }
}
