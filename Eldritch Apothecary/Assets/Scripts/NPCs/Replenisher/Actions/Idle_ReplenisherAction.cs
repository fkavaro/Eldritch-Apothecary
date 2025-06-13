using System;
using System.Collections.Generic;
using UnityEngine;

public class Idle_ReplenisherAction : ABinaryAction<Replenisher>
{
    public Idle_ReplenisherAction(UtilitySystem<Replenisher> utilitySystem)
    : base("Idling", utilitySystem, utilitySystem.controller.replenishThreshold) { }

    protected override bool SetDecisionFactor()
    {
        return true; // Will remain valid action
    }

    public override void StartAction()
    {
        _controller.nodeText.text = "";

        // Move to counter and wait doing nothing
        _controller.SetDestinationSpot(ApothecaryManager.Instance.replenisherSeat);
    }

    public override void UpdateAction()
    {
        if (_controller.HasArrivedAtDestination())
            _controller.ChangeAnimationTo(_controller.sitDownAnim);
    }

    public override bool IsFinished()
    {
        if (ApothecaryManager.Instance.isSomeoneRobbing)
            return false;
        else
            return true;
    }
}