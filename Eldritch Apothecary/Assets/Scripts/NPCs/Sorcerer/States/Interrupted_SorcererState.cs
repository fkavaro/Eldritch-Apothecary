using System.Collections;
using UnityEngine;

public class Interrupted_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    public Interrupted_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
        : base("Interrupted", sfsm) { }

    public override void StartState()
    {
        //Accion no hacer nada hasta que se vaya el gato 
        //_controller.StartCoroutine(WaitForCatToLeave());
        _controller.ChangeAnimationTo(_controller.argueAnim);

    }

    public override void UpdateState()
    {
        //throw new System.NotImplementedException();
    }

    private IEnumerator WaitForCatToLeave()
    {
        while (_controller.CatIsBothering())
        {
            yield return null; // Espera un frame antes de volver a comprobar
        }

        _stateMachine.Pop(); // Vuelve al estado anterior cuando el gato se vaya
    }
}
