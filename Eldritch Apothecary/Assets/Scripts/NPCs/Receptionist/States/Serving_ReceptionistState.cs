using UnityEngine;

public class Serving_ReceptionistState : AState<Receptionist, StackFiniteStateMachine<Receptionist>>
{
    public Serving_ReceptionistState(StackFiniteStateMachine<Receptionist> stackFsm) : base(stackFsm)
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
