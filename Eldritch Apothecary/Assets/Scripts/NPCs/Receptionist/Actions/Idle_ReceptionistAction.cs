using UnityEngine;

public class Idle_ReceptionistAction : AAction<Receptionist>
{
    public Idle_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem, bool isDefault = false)
    : base("Idle", utilitySystem, isDefault) { }

    public override float CalculateUtility()
    {
        // Check if the receptionist is not busy
        if (_behaviourController.IsBusy())
            return 0f; // No utility if busy
        else
            return 1f; // High utility to idle

        // // Check if the receptionist is not busy and can idle
        // if (!controller.attendingState.IsInState() && !controller.attendingState.IsBusy())
        // {
        //     return 1f; // High utility to idle
        // }
        // return 0f; // No utility if not in a valid state
    }

    public override void Execute()
    {
        // Logic to make the receptionist idle
        // controller.idleState.SetIdle(true);
        _behaviourController.SetTargetSpot(ApothecaryManager.Instance.receptionistAttendingPos, _behaviourController.idleAnim);
    }
}
