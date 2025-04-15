using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FiniteStateMachine<TController> : AStateMachine<TController, FiniteStateMachine<TController>> where TController : ABehaviourController<TController>
{
    public FiniteStateMachine(TController controller) : base(controller) { }

    /// <summary>
    /// Sets the initial state of the state machine.
    /// </summary>
    public override void SetInitialState(AState<TController, FiniteStateMachine<TController>> state)
    {
        if (state == currentState) return;

        initialState = state;
        currentState = initialState;
        DebugDecision();
        currentState.StartState();
    }

    /// <summary>
    /// Switchs to another state after exiting the current..
    /// </summary>
    public override void SwitchState(AState<TController, FiniteStateMachine<TController>> state)
    {
        if (state == currentState) return;

        currentState.ExitState();
        currentState = state;
        DebugDecision();
        currentState.StartState();
    }
}
