using UnityEngine;

public class Idle_ReceptionistAction : AAction<Receptionist>
{
    public Idle_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem, bool isDefault = false)
    : base("Idle", utilitySystem, isDefault) { }

    public override float CalculateUtility()
    {
        float utility;

        // Check if the receptionist is busy
        if (_behaviourController.IsBusy())
            utility = 0f; // No utility if busy
        else
            utility = 1f; // Full utility for idle

        //Debug.Log(name + " utility: " + utility);
        return utility;
    }

    public override void StartAction()
    {
        // Move to counter and wait doing nothing
        _behaviourController.SetTargetSpot(ApothecaryManager.Instance.receptionistAttendingPos);
    }

    public override void UpdateAction()
    {
        if (_behaviourController.HasArrived())
            _behaviourController.ChangeAnimationTo(_behaviourController.idleAnim);
    }

    public override bool IsFinished()
    {
        return false; // Idle action never finishes unless interrupted by another action
    }
}
