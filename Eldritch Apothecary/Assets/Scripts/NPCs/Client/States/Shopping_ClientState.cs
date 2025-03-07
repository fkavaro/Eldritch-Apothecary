using UnityEngine;

public class Shopping_ClientState : AClientState
{
    public Shopping_ClientState(FiniteStateMachine fsm, Client clientController) : base(fsm, clientController) { }

    public override void AwakeState()
    {
        // Randomly switch state if client wants any other service
        if (clientController.wantedService != Client.WantedService.OnlyShop &&
            Random.Range(0, 2) == 0)
        {
            fsm.SwitchState(clientController.waitForReceptionist);
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
        // If client has reached the stand
        if (clientController.HasArrived())
        {
            // Take product animation

            // Switch state to wait for receptionist
            fsm.SwitchState(clientController.waitForReceptionist);
        }
    }

    public override void ExitState()
    {
        clientController.StopAgent();
    }
}
