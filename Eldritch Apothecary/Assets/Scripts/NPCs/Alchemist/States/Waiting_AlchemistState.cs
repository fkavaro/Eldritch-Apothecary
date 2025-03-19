using UnityEngine;

public class Waiting_AlchemistState : AnAlchemistState
{
    public Waiting_AlchemistState(StackFiniteStateMachine stackFsm, Alchemist alchemistContext) : base(stackFsm, alchemistContext)
    {
        stateName = "Waiting";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        //Accion esperar a cliente (indefinido)
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return; //Evita que se inicie repetidamente
                                       //
                                       // Si recibe cliente, pasa a esperar ingredientes
        /*if (_alchemistContext.hasOrder())
        {
            _alchemistContext.StartCoroutine(WaitAndSwitchState(_alchemistContext.waitingIngredientsState, "Waiting Ingredients"));
        }*/
    }

    public override void ExitState()
    {
    }
}
