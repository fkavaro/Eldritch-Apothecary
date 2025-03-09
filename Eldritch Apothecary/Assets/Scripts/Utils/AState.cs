using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Base class with common functionalities for all states.
/// </summary>
public abstract class AState
{
    protected FiniteStateMachine fsm;
    protected StackFiniteStateMachine stackFsm;

    /// <summary>
    /// Flag to check if the coroutine has started.
    /// </summary>
    protected bool coroutineStarted = false;

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

    /// <summary>
    /// Coroutine to wait for a specified amount of time before switching to the next state.
    /// </summary>
    protected IEnumerator WaitAndSwitchState(float waitTime, AState nextState, string action = "Executing animation")
    {
        coroutineStarted = true;
        Debug.Log(action + " for " + waitTime + " seconds...");
        yield return new WaitForSeconds(waitTime);
        fsm?.SwitchState(nextState);
        stackFsm?.SwitchState(nextState);
        coroutineStarted = false;
    }
    /// <summary>
    /// Coroutine to wait for a random amount of time before switching to the next state.
    /// </summary>
    protected IEnumerator WaitAndSwitchState(AState nextState, string action = "Acting")
    {
        int waitTime = UnityEngine.Random.Range(5, 21);
        return WaitAndSwitchState(waitTime, nextState, action);
    }

    public virtual void AwakeState() { } // Optionally implemented in subclasses
    public abstract void StartState(); // Implemented in subclasses
    public abstract void UpdateState(); // Implemented in subclasses
    public virtual void ExitState() { } // Optionally in subclasses

    public virtual void OnTriggerEnter(Collider other) { } // Optionally implemented in subclasses
    public virtual void OnTriggerStay(Collider other) { } // Optionally implemented in subclasses
    public virtual void OnTriggerExit(Collider other) { } // Optionally implemented in subclasses

    public virtual void OnCollisionEnter(Collision collision) { } // Optionally implemented in subclasses
    public virtual void OnCollisionStay(Collision collision) { } // Optionally implemented in subclasses
    public virtual void OnCollisionExit(Collision collision) { } // Optionally implemented in subclasses
}