using UnityEngine;

/// <summary>
/// Attends the sorcerer until its service is finished
/// </summary>
public class AtSorcerer_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public AtSorcerer_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("At sorcerer", sfsm) { }

    public override void StartState()
    {
        _behaviourController.SetDestinationSpot(ApothecaryManager.Instance.sorcererSeat);
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached the sorcerer seat
        if (_behaviourController.HasArrivedAtDestination())
            _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, _behaviourController.sitDownAnim, "Sitting down"));
    }
}