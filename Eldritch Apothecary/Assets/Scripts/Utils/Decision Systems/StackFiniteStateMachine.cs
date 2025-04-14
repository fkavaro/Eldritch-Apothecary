using System.Collections.Generic;
using UnityEngine;

public class StackFiniteStateMachine : FiniteStateMachine
{
    /// <summary>
    /// The last active state of the state machine
    /// </summary>
    AState previousState;

    public StackFiniteStateMachine(ABehaviourController controller) : base(controller) { }

    /// <summary>
    /// Switchs to another state after exiting the current,
    /// storing the previous state.
    /// </summary>
    public override void SwitchState(AState state)
    {
        if (state == currentState) return;

        previousState = currentState;
        currentState.ExitState();
        currentState = state;
        Debug();
        currentState.StartState();
    }

    /// <summary>
    /// Switchs to previous state after exiting the current.
    /// </summary>
    public void ReturnToPreviousState()
    {
        currentState.ExitState();
        currentState = previousState;
        Debug();
        currentState.StartState();
    }

    /// <summary>
    /// Switchs back to initial state
    /// </summary>
    public override void Reset()
    {
        SwitchState(initialState);
    }
}
