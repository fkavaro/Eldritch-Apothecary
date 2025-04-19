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
        _controller.SetDestination(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateAction()
    {
        if (_controller.HasArrivedAtDestination())
            _controller.ChangeAnimationTo(_controller.argueAnim);
    }

    public override bool IsFinished()
    {
        // True if client has left
        throw new System.NotImplementedException();
    }
}
