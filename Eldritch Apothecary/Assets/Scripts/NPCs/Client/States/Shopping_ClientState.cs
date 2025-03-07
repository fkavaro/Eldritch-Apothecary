using UnityEngine;

public class Shopping_ClientState : AClientState
{
    public Shopping_ClientState(StackFiniteStateMachine stackFsm, Client clientController) : base(stackFsm, clientController) { }

    public override void AwakeState()
    {
        // Randomly switch state if client wants any other service
        if (clientController.wantedService != Client.WantedService.OnlyShop &&
            Random.Range(0, 5) == 0)
        {
            stackFsm.SwitchState(clientController.waitForReceptionistState);
        }
    }

    public override void StartState()
    {
        // Choose random stand
        clientController.SetTarget(ApothecaryManager.Instance.shopStands
        [Random.Range(0, ApothecaryManager.Instance.shopStands.Length)].position);
    }

    public override void UpdateState()
    {
        // Walk animation

        // If client has reached the stand
        if (clientController.HasArrived())
        {
            // Taking product animation

            // Switch state to waiting in line for receptionist
            stackFsm.SwitchState(clientController.waitForReceptionistState);
        }
    }

    public override void ExitState()
    {
        clientController.StopAgent();
    }
}
