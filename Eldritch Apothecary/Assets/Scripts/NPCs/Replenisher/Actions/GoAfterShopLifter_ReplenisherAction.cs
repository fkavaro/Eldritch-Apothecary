using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GoAfterShopLifter_ReplenisherAction : ABinaryAction<Replenisher>
{
    bool hasFinished = false;
    float previousSpeed;

    public GoAfterShopLifter_ReplenisherAction(UtilitySystem<Replenisher> utilitySystem)
    : base("Going after robber", utilitySystem) { }

    protected override bool SetDecisionFactor()
    {
        return ApothecaryManager.Instance.isSomeoneRobbing;
    }

    public override void StartAction()
    {
        _controller.nodeText.text = "";

        previousSpeed = _controller.speed;
        _controller.speed = 5f;
        hasFinished = false;
        _controller.SetDestination(ApothecaryManager.Instance.entrancePosition.position);
    }

    public override void UpdateAction()
    {
        if (_controller.IsCloseToDestination())
            _controller.PlayAnimationCertainTime(4f, _controller.disbeliefAnim, "Yelling", OnFinished, false);
    }

    public override bool IsFinished()
    {
        return hasFinished;
    }

    void OnFinished()
    {
        _controller.speed = previousSpeed;
        hasFinished = true;
    }
}