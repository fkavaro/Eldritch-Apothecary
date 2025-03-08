using UnityEngine;

public class Shopping_ClientState : AClientState
{
    public Shopping_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext) { }

    public override void AwakeState()
    {
        // Randomly switch state if client wants any other service
        if (clientContext.wantedService != Client.WantedService.OnlyShop &&
            Random.Range(0, 5) == 0)
        {
            stackFsm.SwitchState(clientContext.waitForReceptionistState);
        }
    }

    public override void StartState()
    {
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
        clientContext.StopAgent();
    }
}
