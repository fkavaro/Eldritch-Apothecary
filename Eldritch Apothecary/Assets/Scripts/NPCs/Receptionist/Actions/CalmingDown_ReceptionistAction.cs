using UnityEngine;

public class CalmingDown_ReceptionistAction : AAction<Receptionist>
{
    public CalmingDown_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem, bool isDefault = false)
    : base("Calming down a client", utilitySystem, isDefault) { }

    public override float CalculateUtility()
    {
        // Return 1f if there is a client to calm down, otherwise return 0f
        return 1f;
    }

    public override void StartAction()
    {
        // Approaches the client and calms them down
        _behaviourController.SetTargetPos(ApothecaryManager.Instance.complainingPosition.position, _behaviourController.argueAnim);
    }

    public override void UpdateAction()
    {

    }

    public override bool IsFinished()
    {
        throw new System.NotImplementedException();
    }
}
