using System.Collections;
using UnityEngine;

/// <summary>
/// Base class with common functionalities for all states.
/// </summary>
public abstract class AState<TController, TStateMachine>
    where TController : ABehaviourController<TController>
    where TStateMachine : AStateMachine<TController, TStateMachine>
{
    public string stateName;
    protected TController _behaviourController;
    protected TStateMachine _stateMachine;

    /// <summary>
    /// Flag to check if the coroutine has started.
    /// </summary>
    protected bool _coroutineStarted = false;

    /// <summary>
    /// The time the state has been active.
    /// </summary>
    protected float _stateTime = 0f;

    // Constructor given AStateMachine
    public AState(TStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
        _behaviourController = stateMachine.controller;
    }

    /// <summary>
    /// Coroutine to wait for a specified amount of time before switching to the next state.
    /// </summary>
    protected virtual IEnumerator WaitAndSwitchState(float waitTime, AState<TController, TStateMachine> nextState, string action = "Executing animation")
    {
        _coroutineStarted = true;

        _behaviourController.actionText.text = action + " for " + waitTime + " seconds...";

        yield return new WaitForSeconds(waitTime);

        _stateMachine?.SwitchState(nextState);
        _behaviourController.actionText.text = "";
        _coroutineStarted = false;
    }
    /// <summary>
    /// Coroutine to wait for a random amount of time before switching to the next state.
    /// </summary>
    protected IEnumerator WaitAndSwitchState(AState<TController, TStateMachine> nextState, string action = "Acting")
    {
        int waitTime = Random.Range(5, 21);
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