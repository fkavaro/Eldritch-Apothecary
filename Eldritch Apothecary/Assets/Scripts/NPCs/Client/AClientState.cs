using UnityEngine;


public abstract class AClientState : AState
{
    /// <summary>
    /// The referenced Client gameobject.
    /// </summary>
    protected Client clientContext;

    public AClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm)
    {
        this.clientContext = clientContext;
    }
}
