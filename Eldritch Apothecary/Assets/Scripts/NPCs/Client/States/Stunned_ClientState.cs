using UnityEngine;

/// <summary>
/// Is startled by the grumpy cat. Can lead to a complain.
/// </summary>
public class Stunned_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{

    public Stunned_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Stunned", sfsm) { }

    public override void StartState()
    {
        _behaviourController.StopAgent();

        _behaviourController.StartCoroutine(WaitAndSwitchState(3f, _behaviourController.complainingState, _behaviourController.stunnedAnim, "Stunned")); // TODO: Just wait until animation is executed

        if (_behaviourController.HasReachedMaxScares())
            SwitchState(_behaviourController.complainingState);
        else
            ReturnToPreviousState();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        _behaviourController.ReactivateAgent();
    }
}
