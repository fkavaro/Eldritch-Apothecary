
/// <summary>
/// Common action that makes the humanoid stunned by a cat.
/// </summary>
/// <typeparam name="TController"></typeparam>
public class StunnedByCatAction<TController> : ABinaryAction<TController>
where TController : AHumanoid<TController>
{
    public StunnedByCatAction(UtilitySystem<TController> utilitySystem)
    : base("Stunned by cat", utilitySystem, 0.7f) { }

    protected override bool SetDecisionFactor()
    {
        return _controller.CatIsBothering();
    }

    public override void StartAction()
    {
        _controller.SetIfStopped(true);

        _controller.ChangeAnimationTo(_controller.stunnedAnim);
    }

    public override void UpdateAction()
    {

    }

    public override bool IsFinished()
    {
        if (_controller.IsAnimationFinished())
        {
            _controller.SetIfStopped(false);
            return true; // Action finished
        }
        return false; // Action not finished
    }
}