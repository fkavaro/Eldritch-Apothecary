using UnityEngine;

public class CalmingDown_ReceptionistState : AReceptionistState
{
    public CalmingDown_ReceptionistState(StackFiniteStateMachine stackFsm, Receptionist receptionistContext) : base(stackFsm, receptionistContext)
    {
        stateName = "Calming Down";
    }

    public override void StartState()
    {

    }

    public override void UpdateState()
    {

    }
}
