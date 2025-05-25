using UnityEngine;

public class WaitForClient_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    public WaitForClient_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
        : base("Waiting for client", sfsm) { }

    public override void StartState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}