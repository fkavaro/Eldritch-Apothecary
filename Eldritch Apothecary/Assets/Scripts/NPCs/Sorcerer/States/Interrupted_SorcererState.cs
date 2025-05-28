using UnityEngine;

public class Interrupted_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    public Interrupted_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
        : base("Interrupted", sfsm) { }

    public override void StartState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
