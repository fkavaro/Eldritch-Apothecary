using UnityEngine;

public class Waiting_AlchemistState : AState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public Waiting_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Waiting", stackFsm) { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        //Accion esperar a cliente (indefinido)
    }

    public override void UpdateState()
    {
        /*if (_alchemistContext.hasOrder())
        {
            _alchemistContext.StartCoroutine(WaitAndSwitchState(_alchemistContext.waitingIngredientsState, "Waiting Ingredients"));
        }*/
    }

    public override void ExitState()
    {
    }
}
