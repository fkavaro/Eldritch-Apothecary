using UnityEngine;

public class Complaining_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public Complaining_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Complaining", sfsm) { }

    public override void StartState()
    {
        _behaviourController.SetDestination(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Is close to the complaining position
        if (_behaviourController.IsCloseToDestination())
            _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, _behaviourController.complainAnim, "Complaining"));
    }

    public override void ExitState()
    {

    }
}
