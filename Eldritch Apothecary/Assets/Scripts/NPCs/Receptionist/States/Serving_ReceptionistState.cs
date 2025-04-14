using UnityEngine;

public class Serving_ReceptionistState : AReceptionistState
{
    public Serving_ReceptionistState(StackFiniteStateMachine stackFsm, Receptionist receptionistContext) : base(stackFsm, receptionistContext)
    {
        stateName = "Serving";
    }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {

    }
}
