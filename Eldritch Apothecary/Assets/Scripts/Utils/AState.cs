using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class with common functionalities for all states.
/// </summary>
public abstract class AState
{
    protected FiniteStateMachine fsm;
    protected StackFiniteStateMachine stackFsm;

    /// <summary>
    /// The time the state has been active.
    /// </summary>
    protected float _stateTime = 0f;

    // Constructor given a FiniteStateMachine
    public AState(FiniteStateMachine fsm)
    {
        this.fsm = fsm;
    }

    // Constructor given a StackFiniteStateMachine
    public AState(StackFiniteStateMachine stackFsm)
    {
        this.stackFsm = stackFsm;
    }

    public virtual void AwakeState() { } // Optionally implemented in subclasses
    public abstract void StartState(); // Implemented in subclasses
    public abstract void UpdateState(); // Implemented in subclasses
    public abstract void ExitState(); // Implemented in subclasses

    public virtual void OnTriggerEnter(Collider other) { } // Optionally implemented in subclasses
    public virtual void OnTriggerStay(Collider other) { } // Optionally implemented in subclasses
    public virtual void OnTriggerExit(Collider other) { } // Optionally implemented in subclasses

    public virtual void OnCollisionEnter(Collision collision) { } // Optionally implemented in subclasses
    public virtual void OnCollisionStay(Collision collision) { } // Optionally implemented in subclasses
    public virtual void OnCollisionExit(Collision collision) { } // Optionally implemented in subclasses
}