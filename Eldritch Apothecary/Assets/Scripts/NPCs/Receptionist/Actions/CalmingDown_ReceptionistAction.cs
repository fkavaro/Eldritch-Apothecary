using UnityEngine;

public class CalmingDown_ReceptionistAction : ABinaryAction<Receptionist>
{
    public CalmingDown_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem)
    : base("Calming down a client", utilitySystem) { }

    protected override bool SetDecisionFactor()
    {
        return ApothecaryManager.Instance.SomeoneComplaining();
    }

    public override void StartAction()
    {
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
