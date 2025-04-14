using UnityEngine;

/// <summary>
/// Is startled by the grumpy cat. Can lead to a complain.
/// </summary>
public class Stunned_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{

    public Stunned_ClientState(StackFiniteStateMachine<Client> sfsm) : base(sfsm)
    {
        stateName = "Stunned";
    }

    public override void StartState()
    {
        _behaviourController.StopAgent();

        _behaviourController.ChangeAnimationTo(_behaviourController.stunnedAnim);

        _behaviourController.StartCoroutine(WaitAndSwitchState(3f, _behaviourController.complainingState, "Stunned")); // TODO: Just wait until animation is executed

        if (_behaviourController.HasReachedMaxScares())
            _stateMachine.SwitchState(_behaviourController.complainingState);
        else
            _stateMachine.ReturnToPreviousState();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        _behaviourController.ReactivateAgent();
    }
}
