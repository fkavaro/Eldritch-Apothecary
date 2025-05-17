using Unity.VisualScripting;
using UnityEngine;

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
        _controller.CoroutineFinishedEvent += FinishedComplaining;

        // Still in waiting queue 
        if (ApothecaryManager.Instance.waitingQueue.Contains(_controller))
            // Leave the queue for next turn
            ApothecaryManager.Instance.waitingQueue.NextTurn();

        // Go to complaining position
        _controller.SetDestination(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateAction()
    {
        if (finishedComplaining) return;

        // Is close to the complaining position
        if (_controller.IsCloseToDestination())
            _controller.StartCoroutine(_controller.PlayAnimationRandomTime(_controller.complainAnim, "Complaining"));
    }

    public override bool IsFinished()
    {
        if (finishedComplaining)
        {
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