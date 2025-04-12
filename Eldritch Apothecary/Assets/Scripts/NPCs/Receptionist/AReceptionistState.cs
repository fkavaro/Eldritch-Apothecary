
public abstract class AReceptionistState : AState
{
    /// <summary>
    /// The referenced Receptionist gameobject.
    /// </summary>
    protected Receptionist _receptionistContext;

    public AReceptionistState(StackFiniteStateMachine stackFsm, Receptionist receptionistContext) : base(stackFsm)
    {
        _behaviourController = receptionistContext;
        _receptionistContext = receptionistContext;
    }
}
