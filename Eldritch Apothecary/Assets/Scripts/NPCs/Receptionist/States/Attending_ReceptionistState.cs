using UnityEngine;

public class Attending_RececptionistState : AReceptionistState
{
    public Attending_RececptionistState(StackFiniteStateMachine stackFsm, Receptionist receptionistContext) : base(stackFsm, receptionistContext)
    {
        stateName = "Attending";
    }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {

    }
}
