
public class Complain_ClientAction : ABinaryAction<Client>
{
    bool finishedComplaining = false;

    public Complain_ClientAction(UtilitySystem<Client> utilitySystem)
        : base("Complaining", utilitySystem) { }

    protected override bool SetDecisionFactor()
    {
        return _controller.HasReachedMaxScares() || _controller.WaitedTooLong();
    }

    public override void StartAction()
    {
        finishedComplaining = false;
        _controller.animationText.text = "";

        // Finished complaining when coroutine finished
        _controller.CoroutineFinishedEvent += () => finishedComplaining = true; ;

        // Still in waiting queue 
        if (ApothecaryManager.Instance.waitingQueue.Contains(_controller))
            // Leave the queue for next turn
            ApothecaryManager.Instance.waitingQueue.NextTurn();

        // Go to complaining position
        _controller.SetDestination(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateAction()
    {
        // Is close to the complaining position and hasn't started complaining yet
        if (_controller.IsCloseTo(ApothecaryManager.Instance.complainingPosition.position, 3f))
        {
            _controller.SetIfStopped(true);
            _controller.StartCoroutine(_controller.PlayAnimationRandomTime(_controller.complainAnim, "Complaining"));
        }
    }

    public override bool IsFinished()
    {
        if (finishedComplaining)
        {
            _controller.animationText.text = "";
            _controller.fsmAction.ForceState(_controller.leavingState);
            finishedComplaining = false;
            return true;
        }
        else return false;
    }
}