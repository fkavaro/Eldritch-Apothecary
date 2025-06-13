
public class Complain_ClientAction : ABinaryAction<Client>
{
    bool finishedComplaining = false;

    public Complain_ClientAction(UtilitySystem<Client> utilitySystem)
        : base("Complaining", utilitySystem) { }

    protected override bool SetDecisionFactor()
    {
        return _controller.TooScared() || _controller.WaitedTooLong();
    }

    public override void StartAction()
    {
        _controller.animationText.text = "";

        // If the client leaving is the next in line to the sorcerer, adds one to the sorcerer turn
        //if ((_controller.wantedService == Client.WantedService.SPELL) && (_controller.turnNumber == ApothecaryManager.Instance.currentSorcererTurn))
        //{
        //    ApothecaryManager.Instance.NextSorcererTurn();
        //    // Updates sorcerer clients list
        //    ApothecaryManager.Instance.sorcererClientsQueue.Remove(_controller);
        //}

        // Still in waiting queue 
        if (ApothecaryManager.Instance.waitingQueue.Contains(_controller))
            // Leave the queue for next turn
            ApothecaryManager.Instance.waitingQueue.NextTurn();

        // Go to complaining position
        _controller.SetDestination(ApothecaryManager.Instance.complainingPosition.position);
        _controller.isExecutionPaused = false;
    }

    public override void UpdateAction()
    {
        // Is close to the complaining position and hasn't started complaining yet
        if (_controller.IsCloseTo(ApothecaryManager.Instance.complainingPosition.position, 3f))
        {
            _controller.SetIfStopped(true);
            _controller.transform.LookAt(ApothecaryManager.Instance.receptionist.transform.position);
            ApothecaryManager.Instance.AddToComplains(_controller);

            if (ApothecaryManager.Instance.receptionist.canCalmDown)
                _controller.PlayAnimationRandomTime(_controller.complainAnim, "Complaining", () => finishedComplaining = true);
            else
                _controller.ChangeAnimationTo(_controller.waitAnim);
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