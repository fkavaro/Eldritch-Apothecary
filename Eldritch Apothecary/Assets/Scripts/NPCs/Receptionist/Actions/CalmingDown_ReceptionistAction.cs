using System;
using UnityEngine;

public class CalmingDown_ReceptionistAction : ABinaryAction<Receptionist>
{
    public CalmingDown_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem)
    : base("Calming down a client", utilitySystem) { }

    protected override bool SetDecisionFactor()
    {
        return ApothecaryManager.Instance.IsSomeoneComplaining();
    }

    public override void StartAction()
    {
        // Approaches the client and calms them down
        _controller.SetDestination(ApothecaryManager.Instance.receptionistCalmDownPosition.transform.position);
    }

    public override void UpdateAction()
    {
        if (_controller.HasArrivedAtDestination() && ApothecaryManager.Instance.IsSomeoneComplaining())
        {
            _controller.canCalmDown = true;
            _controller.transform.LookAt(ApothecaryManager.Instance.CurrentComplainingClient().transform.position);
            _controller.ChangeAnimationTo(_controller.argueAnim);
        }
        else
            _controller.canCalmDown = false;
    }

    public override bool IsFinished()
    {
        // Noone is complaining
        return !ApothecaryManager.Instance.IsSomeoneComplaining();
    }
}
