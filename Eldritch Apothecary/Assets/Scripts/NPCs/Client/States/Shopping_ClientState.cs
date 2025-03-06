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
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
}
