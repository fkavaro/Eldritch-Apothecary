
/// <summary>
/// Stack-based Finite State Machine implementation for controlling a behaviour.
/// </summary>
public class StackFiniteStateMachine<TController> : AStateMachine<TController, StackFiniteStateMachine<TController>> where TController : ABehaviourController<TController>
{
    /// <summary>
    /// The last active state of the state machine
    /// </summary>
    AState<TController, StackFiniteStateMachine<TController>> previousState;

    public StackFiniteStateMachine(TController controller) : base(controller) { }

    #region INHERITED METHODS
    /// <summary>
    /// Sets the initial state of the state machine.
    /// </summary>
    public override void SetInitialState(AState<TController, StackFiniteStateMachine<TController>> state)
    {
        if (state == currentState) return;

        initialState = state;
        currentState = initialState;
        previousState = initialState;
        DebugDecision();
        currentState.StartState();
    }

    /// <summary>
    /// Switchs to another state after exiting the current,
    /// storing the previous state.
    /// </summary>
    public override void SwitchState(AState<TController, StackFiniteStateMachine<TController>> state)
    {
        if (state == currentState) return;

        previousState = currentState;
        currentState.OnExitState();
        currentState = state;
        DebugDecision();
        currentState.StartState();
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Switchs to previous state after exiting the current.
    /// </summary>
    public void ReturnToPreviousState()
    {
        SwitchState(previousState);
    }
    #endregion
}