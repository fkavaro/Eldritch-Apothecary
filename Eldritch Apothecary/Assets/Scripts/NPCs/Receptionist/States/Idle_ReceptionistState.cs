using UnityEngine;

public class Idle_ReceptionistState : AReceptionistState
{
    public Idle_ReceptionistState(StackFiniteStateMachine stackFsm, Receptionist receptionistContext) : base(stackFsm, receptionistContext)
    {
        stateName = "Idle";
    }

    public override void StartState()
    {
        _receptionistContext.SetTarget(ApothecaryManager.Instance.receptionistAttendingPos);
    }

    public override void UpdateState()
    {
        // Arrived at attending position
        if (_receptionistContext.HasArrived())
            _receptionistContext.ChangeAnimationTo(_receptionistContext.idleAnim);
    }
}
