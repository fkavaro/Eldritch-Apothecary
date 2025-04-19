
public class Complain_ClientAction : ABinaryAction<Client>
{
    bool finishedComplaining = false;

    public Complain_ClientAction(UtilitySystem<Client> utilitySystem)
        : base("Complain", utilitySystem) { }

    protected override bool SetDecisionFactor()
    {
        return _controller.HasReachedMaxScares() || _controller.WaitedTooLong();
    }

    public override void StartAction()
    {
        _controller.CoroutineFinishedEvent += FinishedComplaining;
        _controller.SetDestination(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateAction()
    {
        // Is close to the complaining position
        if (_controller.IsCloseToDestination())
            _controller.StartCoroutine(_controller.PlayAnimationRandomTime(_controller.complainAnim, "Complaining"));
    }

    public override bool IsFinished()
    {
        return finishedComplaining; // Action never finishes unless interrupted by another action
    }

    void FinishedComplaining()
    {
        finishedComplaining = true;
    }
}