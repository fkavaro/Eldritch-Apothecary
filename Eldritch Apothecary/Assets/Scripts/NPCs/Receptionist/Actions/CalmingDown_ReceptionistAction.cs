using UnityEngine;

public class CalmingDown_ReceptionistAction : AAction<Receptionist>
{
    public CalmingDown_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem, bool isDefault = false)
    : base("Calming down a client", utilitySystem, isDefault) { }

    public override float CalculateUtility()
    {
        float utility;

        // Return 1f if there is a client to calm down, otherwise return 0f
        utility = 0f;

        //Debug.Log(name + " utility: " + utility);
        return utility;
    }

    public override void StartAction()
    {
        Debug.Log("Receptionist is calming down a client");

        // Approaches the client and calms them down
        _behaviourController.SetTargetPos(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateAction()
    {
        if (_behaviourController.HasArrived())
            _behaviourController.ChangeAnimationTo(_behaviourController.argueAnim);
    }

    public override bool IsFinished()
    {
        // True if client has left
        throw new System.NotImplementedException();
    }
}
