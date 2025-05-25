using UnityEngine;

public class WaitForIngredient_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    public WaitForIngredient_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
        : base("Waiting for ingredient", sfsm) { }

    public override void StartState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
