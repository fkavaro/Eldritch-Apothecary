using System.Collections;
using UnityEngine;

/// <summary>
/// Plays yell animation until the cat leaves the table
/// </summary>
public class Interrupted_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    public Interrupted_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
        : base("Interrupted", sfsm) { }

    public override void StartState()
    {
        // Yell animation while the cat is on the table
        _controller.ChangeAnimationTo(_controller.yellAnim);
    }

    public override void UpdateState()
    {
        // Keeps complaining until the cat leaves the table
    }
}
