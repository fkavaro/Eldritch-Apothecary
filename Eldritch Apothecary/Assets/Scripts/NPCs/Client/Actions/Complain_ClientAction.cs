using UnityEngine;

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

        // Leave the queue for next turn and go to the complaining position
        ApothecaryManager.Instance.waitingQueue.NextTurn();
        _controller.SetDestination(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateAction()
    {
        // Is close to the complaining position
        if (_controller.HasArrivedAtDestination())
            _controller.StartCoroutine(_controller.PlayAnimationRandomTime(_controller.complainAnim, "Complaining"));
    }

    public override bool IsFinished()
    {
        if (finishedComplaining)
        {
            Debug.Log("Finished complaining");
            _controller.ForceState(_controller.leavingState);
            return true; // Action finished
        }
        else return false; // Action not finished
    }

    void FinishedComplaining()
    {
        finishedComplaining = true;
    }
}