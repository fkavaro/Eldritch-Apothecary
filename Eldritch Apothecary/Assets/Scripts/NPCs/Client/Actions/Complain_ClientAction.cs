
using UnityEngine;

public class Complain_ClientAction : ABinaryAction<Client>
{
    bool finishedComplaining = false;
    Transform complainPosition, queueExitPosition;

    public Complain_ClientAction(UtilitySystem<Client> utilitySystem)
        : base("Complaining", utilitySystem) { }

    protected override bool SetDecisionFactor()
    {
        return _controller.TooScared() || _controller.WaitedTooLong();
    }

    public override void StartAction()
    {
        _controller.animationText.text = "";
        complainPosition = ApothecaryManager.Instance.complainingPosition;
        queueExitPosition = ApothecaryManager.Instance.queueExitPosition;

        // Still in waiting queue 
        if (ApothecaryManager.Instance.waitingQueue.Contains(_controller))
            // Leave the queue for next turn
            ApothecaryManager.Instance.waitingQueue.NextTurn();

        // Was shopping
        if (_controller.fsmAction.IsCurrentState(_controller.fsmAction.shoppingState))
            _controller.SetDestination(queueExitPosition.position); // First go to queue exit
        else //Wasn't shopping
            _controller.SetDestination(complainPosition.position); // Go directly to complaining position

        _controller.isExecutionPaused = false;
    }

    public override void UpdateAction()
    {
        // Is close to the complaining position and hasn't started complaining yet
        if (_controller.IsCloseTo(complainPosition.position, 3f))
        {
            _controller.SetIfStopped(true);
            _controller.transform.LookAt(ApothecaryManager.Instance.receptionist.transform.position);
            ApothecaryManager.Instance.AddToComplains(_controller);

            if (ApothecaryManager.Instance.receptionist.canCalmDown)
                _controller.PlayAnimationRandomTime(_controller.complainAnim, "Complaining", () => finishedComplaining = true);
            else
                _controller.ChangeAnimationTo(_controller.waitAnim);
        }
        else if (_controller.IsCloseTo(queueExitPosition.position, 3f))
        {
            _controller.SetDestination(complainPosition.position); // Go directly to complaining position
        }
    }

    public override bool IsFinished()
    {
        if (finishedComplaining)
        {
            _controller.animationText.text = "";
            ApothecaryManager.Instance.RemoveFromComplains(_controller);
            _controller.fsmAction.ForceState(_controller.fsmAction.leavingState);
            finishedComplaining = false;
            return true;
        }
        else return false;
    }
}