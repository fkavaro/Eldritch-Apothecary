using UnityEngine;

public abstract class AClientState : AState
{
    protected Client clientController;

    public AClientState(FiniteStateMachine fsm, Client clientController) : base(fsm)
    {
        this.clientController = clientController;
    }
}
