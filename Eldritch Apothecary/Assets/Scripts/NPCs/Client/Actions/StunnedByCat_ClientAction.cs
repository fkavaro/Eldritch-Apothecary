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
        //Debug.Log(_controller.name + " stunned by cat");

        _controller.scaresCount++;

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
            //Debug.Log(_controller.name + " not stunned by cat anymore");
            _controller.SetIfStopped(false);
            _controller.ChangeToPreviousAnimation();
            return true; // Action finished
        }
        return false; // Action not finished
    }
}