using UnityEngine;

public class Attending_ReceptionistAction : AAction<Receptionist>
{
    public Attending_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem, bool isDefault = false)
    : base("Attending clients", utilitySystem, isDefault) { }

    public override float CalculateUtility()
    {
        // Return the time that the next client has been waiting
        return 2f;
    }

    public override void Execute()
    {
        // Move to counter and attend the client
        _behaviourController.SetTargetSpot(ApothecaryManager.Instance.receptionistAttendingPos, _behaviourController.talkAnim);
    }
}
