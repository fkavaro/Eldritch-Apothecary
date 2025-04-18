using UnityEngine;

public class Idle_ReceptionistAction : ABinaryAction<Receptionist>
{
    public Idle_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem)
    : base("Idle", utilitySystem, true) { }

    protected override bool SetDecisionFactor()
    {
        return _behaviourController.IsBusy();
    }

    public override void StartAction()
    {
        // Move to counter and wait doing nothing
        _behaviourController.SetDestinationSpot(ApothecaryManager.Instance.receptionistAttendingPos);
    }

    public override void UpdateAction()
    {
        if (_behaviourController.HasArrivedAtDestination())
            _behaviourController.ChangeAnimationTo(_behaviourController.idleAnim);
    }

    public override bool IsFinished()
    {
        return false; // Idle action never finishes unless interrupted by another action
    }
}
