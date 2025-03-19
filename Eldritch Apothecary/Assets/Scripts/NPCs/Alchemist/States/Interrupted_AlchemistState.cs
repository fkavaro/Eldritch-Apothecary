using System.Collections;
using UnityEngine;

public class Interrupted_AlchemistState : AnAlchemistState
{
    public Interrupted_AlchemistState(StackFiniteStateMachine stackFsm, Alchemist alchemistContext) : base(stackFsm, alchemistContext)
    {
        stateName = "Interrupted";
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        //Accion no hacer nada hasta que se vaya el gato 
        _alchemistContext.StartCoroutine(WaitForCatToLeave());
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
    }

    private IEnumerator WaitForCatToLeave()
    {
        while (Vector3.Distance(_alchemistContext.transform.position, ApothecaryManager.Instance.cat.transform.position) < _alchemistContext.minDistanceToCat)
        {
            yield return null; // Espera un frame antes de volver a comprobar
        }

        _stackFsm.ReturnToPreviousState(); // Vuelve al estado anterior cuando el gato se vaya
    }
}
