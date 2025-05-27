using UnityEngine;

/// <summary>
/// Common action that makes the humanoid stunned by a cat.
/// </summary>
/// <typeparam name="TController"></typeparam>
public class StunnedByCat_ClientAction : ABinaryAction<Client>
{
    public StunnedByCat_ClientAction(UtilitySystem<Client> utilitySystem)
    : base("Stunned by cat", utilitySystem, 0.7f) { }

    protected override bool SetDecisionFactor()
    {
        return _controller.CatIsBothering();
    }

    public override void StartAction()
    {
        _controller.scaresCount++;
        _controller.lastScareTime = Time.time;
        _controller.ChangeAnimationTo(_controller.stunnedAnim, true);
    }

    public override void UpdateAction()
    {

    }

    public override bool IsFinished()
    {
        if (_controller.IsAnimationFinished())
        {
            _controller.ChangeToPreviousAnimation();
            return true;
        }
        return false;
    }
}