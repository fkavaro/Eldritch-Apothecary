
public class Idle_ReceptionistAction : ABinaryAction<Receptionist>
{
    public Idle_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem)
    : base("Idle", utilitySystem, true) { }

    protected override bool SetDecisionFactor()
    {
        return _controller.IsBusy();
    }

    public override void StartAction()
    {
        // Move to counter and wait doing nothing
        _controller.SetDestinationSpot(ApothecaryManager.Instance.receptionistAttendingPos);
    }

    public override void UpdateAction()
    {
        if (_controller.HasArrivedAtDestination())
            _controller.ChangeAnimationTo(_controller.idleAnim);
    }

    public override bool IsFinished()
    {
        return false; // Idle action never finishes unless interrupted by another action
    }
}
