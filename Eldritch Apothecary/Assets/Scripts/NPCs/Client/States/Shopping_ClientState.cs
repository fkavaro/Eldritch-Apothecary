using UnityEngine;

public class Shopping_ClientState : AClientState
{
    public Shopping_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext) { }

    public override void StartState()
    {
        // Switch state if client wants any other service: sorcerer or alchemist
        if (clientContext.wantedService != Client.WantedService.OnlyShop)
            stackFsm.SwitchState(clientContext.waitForReceptionistState);
        else
            // Choose random stand
            clientContext.SetTarget(ApothecaryManager.Instance.RandomShopStand());
    }

    public override void UpdateState()
    {
        if (clientContext.HasArrived() && !coroutineStarted)
        {
            clientContext.ChangeAnimationTo(clientContext.PickUp);
            clientContext.StartCoroutine(WaitAndSwitchState(clientContext.waitForReceptionistState));
        }
    }

    public override void ExitState()
    {
        //clientContext.StopAgent();
    }
}
