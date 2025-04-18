
/// <summary>
/// Action that runs a finite state machine (FSM) by an Utility System.
/// </summary>
public class EstateMachineAction<TController, TStateMachine> : ABinaryAction<TController>
where TController : ABehaviourController<TController>
where TStateMachine : AStateMachine<TController, TStateMachine>
{
    TStateMachine _stateMachine;

    public EstateMachineAction(UtilitySystem<TController> utilitySystem, TStateMachine stateMachine)
        : base("FSM", utilitySystem, true)
    {
        _stateMachine = stateMachine;
    }

    protected override bool SetDecisionFactor()
    {
        return true;
    }

    public override void StartAction()
    {

    }

    public override void UpdateAction()
    {
        _stateMachine.Update();
    }

    public override bool IsFinished()
    {
        return false; // FSM action never finishes unless interrupted by another action
    }

}