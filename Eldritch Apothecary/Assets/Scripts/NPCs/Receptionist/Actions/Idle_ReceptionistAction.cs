using System;
using UnityEngine;

public class Idle_ReceptionistAction : ABinaryAction<Receptionist>
{
    public Idle_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem)
    : base("Idle", utilitySystem, 0.1f) { }

    protected override bool SetDecisionFactor()
    {
        return !_controller.IsBusy() && !ApothecaryManager.Instance.IsSomeoneComplaining();
    }

    public override void StartAction()
    {
        // Move to counter and wait doing nothing
        _controller.SetDestinationSpot(ApothecaryManager.Instance.receptionistAttendingSpot);
    }

    public override void UpdateAction()
    {
        if (_controller.HasArrivedAtDestination())
            _controller.ChangeAnimationTo(_controller.idleAnim);
    }

    public override bool IsFinished()
    {
        return true; // Allows avaluation of other actions
    }
}
