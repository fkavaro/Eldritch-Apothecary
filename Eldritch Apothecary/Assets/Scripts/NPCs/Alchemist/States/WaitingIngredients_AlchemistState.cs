using System.Collections;
using UnityEngine;

public class WaitingIngredients_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public WaitingIngredients_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
        : base("Waiting Ingredients", stackFsm) { }

    public override void StartState()
    {
        _controller.ChangeAnimationTo(_controller.idleAnim); // Esperando ingredientes
        _controller.StartCoroutine(WaitingForIngredients());
    }

    public override void UpdateState() { }
    public override void ExitState() { }

    private IEnumerator WaitingForIngredients()
    {
        float waitTime = 3f;
        yield return new WaitForSeconds(waitTime);

        // Reintenta coger ingredientes
        _stateMachine.SwitchState(_controller.pickingUpIngredientsState);
    }
}
