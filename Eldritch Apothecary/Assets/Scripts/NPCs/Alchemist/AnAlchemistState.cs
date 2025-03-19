using UnityEngine;
using System.Collections;

public abstract class AnAlchemistState : AState
{

    protected Alchemist _alchemistContext;
    
    public AnAlchemistState(StackFiniteStateMachine stackfsm, Alchemist alchemistContext) : base(stackfsm)
    {
        _behaviourController = alchemistContext;
        _alchemistContext = alchemistContext;
    }
}
