
/// <summary>
/// Action that runs a finite state machine (FSM) by an Utility System.
/// </summary>
public class StateMachineAction<TController, TStateMachine> : ABinaryAction<TController>
where TController : ABehaviourController<TController>
where TStateMachine : AStateMachine<TController, TStateMachine>
{
    TStateMachine _stateMachine;

    public StateMachineAction(UtilitySystem<TController> utilitySystem, TStateMachine stateMachine)
        : base("FSM", utilitySystem, 0.5f)
    {
        _stateMachine = stateMachine;
    }

    protected override bool SetDecisionFactor()
    {
        return true; // Will remain valid action
    }

    public override void StartAction()
    {
        _stateMachine.Start();
    }

    public override void UpdateAction()
    {
        _stateMachine.Update();
    }

    public override bool IsFinished()
    {
        return false; // FSM action never finishes unless interrupted by another action
    }

    public override string DebugDecision()
    {
        return _stateMachine.GetCurrentStateName();
    }

    public override void Reset()
    {
        _utilitySystem.CurrentAsDefaultAction();
        _stateMachine.Reset();
    }
}