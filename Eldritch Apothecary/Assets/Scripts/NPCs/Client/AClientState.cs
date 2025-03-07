using UnityEngine;

public abstract class AClientState : AState
{
    protected Client clientController;

    public AClientState(StackFiniteStateMachine stackFsm, Client clientController) : base(stackFsm)
    {
        this.clientController = clientController;
    }
}
