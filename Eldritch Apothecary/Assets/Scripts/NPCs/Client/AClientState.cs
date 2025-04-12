
public abstract class AClientState : AState
{
    /// <summary>
    /// The referenced Client gameobject.
    /// </summary>
    protected Client _clientContext;

    public AClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm)
    {
        _behaviourController = clientContext;
        _clientContext = clientContext;
    }
}
