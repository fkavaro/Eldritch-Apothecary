using UnityEngine;

public class AttendingClients_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    public AttendingClients_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
    : base("Attending Clients", sfsm) { }

    public override void StartState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
