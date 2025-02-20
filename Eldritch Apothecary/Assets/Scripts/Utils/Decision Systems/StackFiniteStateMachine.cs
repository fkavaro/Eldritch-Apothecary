using System.Collections.Generic;
using UnityEngine;

public class StackFiniteStateMachine : FiniteStateMachine
{
    /// <summary>
    /// The last active state of the state machine
    /// </summary>
    public AState previousState;

    /// <summary>
    /// Switchs to another state after exiting the current,
    /// storing the previous state.
    /// </summary>
    public override void SwitchState(AState state)
    {
        previousState = currentState;
        currentState.ExitState();
        currentState = state;
        currentState.StartState();

        if (debug) Debug.LogWarning(currentState.ToString());
    }

    /// <summary>
    /// Switchs to previous state after exiting the current.
    /// </summary>
    public void ReturnToPreviousState()
    {
        currentState.ExitState();
        currentState = previousState;
        currentState.StartState();

        if (debug) Debug.LogWarning(currentState.ToString());
    }
}
