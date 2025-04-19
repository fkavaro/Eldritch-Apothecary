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
        _controller.SetIfStopped(true);

        // _controller.StartCoroutine(SwitchStateAfterCertainTime(3f, _controller.complainingState, _controller.stunnedAnim, "Stunned")); // TODO: Just wait until animation is executed

        // if (_controller.HasReachedMaxScares())
        //     SwitchState(_controller.complainingState);
        // else
        //     ReturnToPreviousState();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        _controller.SetIfStopped(false);
    }
}
