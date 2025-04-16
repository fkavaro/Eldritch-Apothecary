using System;
using UnityEngine;

public class Attending_ReceptionistAction : ALinearAction<Receptionist>
{
    bool _clientHasChanged = false; // Flag to check if the next client is ready to be served

    public Attending_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem)
    : base("Attending clients", utilitySystem) { }

    protected override float SetDecisionFactor()
    {
        // Return the time that the next client has been waiting
        // Normalized between 0 and the maximun waiting time of the client
        return ApothecaryManager.Instance.waitingQueue.GetNextClientNormalizedWaitingTime();
    }

    public override void StartAction()
    {
        // Subscribe to the event invoked when the client at counter has changed
        ApothecaryManager.Instance.waitingQueue.NextTurnEvent += NextClient;

        _clientHasChanged = false; // Reset the flag when starting the action

        // Move to counter and attend the client
        _behaviourController.SetTargetSpot(ApothecaryManager.Instance.receptionistAttendingPos);
    }

    public override void UpdateAction()
    {
        if (_behaviourController.HasArrived())
            _behaviourController.ChangeAnimationTo(_behaviourController.talkAnim);
    }

    public override bool IsFinished()
    {
        return _clientHasChanged; // True if client has changed
    }

    private void NextClient(Client client)
    {
        // True because this has been invoked when it has changed
        _clientHasChanged = true; // Set the flag to true when the next client is ready to be served
    }
}
