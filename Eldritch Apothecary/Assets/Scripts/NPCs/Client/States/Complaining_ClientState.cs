using UnityEngine;

public class Complaining_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{
    public Complaining_ClientState(StackFiniteStateMachine<Client> sfsm) : base(sfsm)
    {
        stateName = "Complaining";
    }

    public override void StartState()
    {
        _behaviourController.SetTargetPos(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached the complaining position
        if (_behaviourController.HasArrived())
        {
            _behaviourController.ChangeAnimationTo(_behaviourController.complainAnim);
            _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, "Complaining"));
        }
    }

    public override void ExitState()
    {

    }
}
