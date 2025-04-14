using UnityEngine;

public class Attending_ReceptionistState : AState<Receptionist, StackFiniteStateMachine<Receptionist>>
{
    public Attending_ReceptionistState(StackFiniteStateMachine<Receptionist> stackFsm) : base(stackFsm)
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
