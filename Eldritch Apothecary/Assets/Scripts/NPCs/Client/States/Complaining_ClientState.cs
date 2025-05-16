using UnityEngine;

// NOT IN USE
public class Complaining_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public Complaining_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Complaining", sfsm) { }

    public override void StartState()
    {
        _controller.SetDestination(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateState()
    {
        // Is close to the complaining position
        if (_controller.IsCloseToDestination())
            _controller.StartCoroutine(SwitchStateAfterRandomTime(_controller.leavingState, _controller.complainAnim, "Complaining"));
    }

    public override void ExitState()
    {

    }
}
