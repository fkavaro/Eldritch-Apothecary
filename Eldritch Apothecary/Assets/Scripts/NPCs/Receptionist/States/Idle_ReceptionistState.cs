using UnityEngine;

public class Idle_ReceptionistState : AState<Receptionist, StackFiniteStateMachine<Receptionist>>
{
    public Idle_ReceptionistState(StackFiniteStateMachine<Receptionist> stackFsm) : base(stackFsm)
    {
        stateName = "Idle";
    }

    public override void StartState()
    {
        _behaviourController.SetTargetSpot(ApothecaryManager.Instance.receptionistAttendingPos);
    }

    public override void UpdateState()
    {
        // Arrived at attending position
        if (_behaviourController.HasArrived())
            _behaviourController.ChangeAnimationTo(_behaviourController.idleAnim);
    }
}
