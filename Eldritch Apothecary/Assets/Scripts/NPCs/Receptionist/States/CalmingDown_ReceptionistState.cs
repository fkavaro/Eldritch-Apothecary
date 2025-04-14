using UnityEngine;

public class CalmingDown_ReceptionistState : AState<Receptionist, StackFiniteStateMachine<Receptionist>>
{
    public CalmingDown_ReceptionistState(StackFiniteStateMachine<Receptionist> stackFsm) : base(stackFsm)
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
