
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Stack-based Finite State Machine implementation for controlling a behaviour.
/// </summary>
public class StackFiniteStateMachine<TController> : AStateMachine<TController, StackFiniteStateMachine<TController>> where TController : ABehaviourController<TController>
{
    /// <summary>
    /// The last active state of the state machine
    /// </summary>
    //AState<TController, StackFiniteStateMachine<TController>> previousState;

    Stack<AState<TController, StackFiniteStateMachine<TController>>> _stateStack = new();

    public StackFiniteStateMachine(TController controller) : base(controller) { }

    #region INHERITED METHODS
    public override void Start()
    {
        // Previous state will be initial state at start and after changing
        // will be able to return the previous state from Utility System.Reset()
        Pop();
    }

    /// <summary>
    /// Sets the initial state of the state machine.
    /// </summary>
    public override void SetInitialState(AState<TController, StackFiniteStateMachine<TController>> state)
    {
        if (state == currentState) return;

        initialState = state;
        currentState = state;
        PushCurrentState();
    }

    /// <summary>
    /// Switchs to another state after exiting the current,
    /// storing it in the stack.
    /// </summary>
    public override void SwitchState(AState<TController, StackFiniteStateMachine<TController>> newState)
    {
        if (newState == currentState) return;

        PushCurrentState();
        currentState?.OnExitState();
        currentState = newState;
        DebugDecision();
        currentState?.StartState();
    }


    public override void ForceState(AState<TController, StackFiniteStateMachine<TController>> newState)
    {
        if (newState == currentState) return;

        PushCurrentState();
        // Don't exit the current state, just set and start the new one
        currentState = newState;
        DebugDecision();
        currentState.StartState();
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Save the last state in the stack.
    /// </summary>
    public void PushCurrentState()
    {
        _stateStack.Push(currentState);
        //SwitchState(targetState);
    }

    /// <summary>
    /// Return to the last state saved in the stack, if exists.
    /// </summary>
    public void Pop()
    {
        if (_stateStack.Count == 0) return;

        var targetState = _stateStack.Pop();
        SwitchState(targetState);
    }
    #endregion
}