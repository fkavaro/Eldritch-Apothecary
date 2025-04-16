using UnityEngine;

public class Idle_ReceptionistAction : AAction<Receptionist>
{
    public Idle_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem, bool isDefault = false)
    : base("Idle", utilitySystem, isDefault) { }

    public override float CalculateUtility()
    {
        // Check if the receptionist is busy
        if (_behaviourController.IsBusy())
            return 0f; // No utility if busy
        else
            return 1f; // High utility to idle
    }

    public override void StartAction()
    {
        // Move to counter and wait doing nothing
        _behaviourController.SetTargetSpot(ApothecaryManager.Instance.receptionistAttendingPos, _behaviourController.idleAnim);
    }

    public override void UpdateAction()
    {

    }

    public override bool IsFinished()
    {
        return false; // Idle action never finishes unless interrupted by another action
    }
}
