using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for NPC states, allowing to handle animations.
/// </summary>
/// <typeparam name="TController"></typeparam>
/// <typeparam name="TStateMachine"></typeparam>
public abstract class ANPCState<TController, TStateMachine> : AState<TController, TStateMachine>
    where TController : ANPC<TController>
    where TStateMachine : AStateMachine<TController, TStateMachine>
{
    public ANPCState(string name, TStateMachine stateMachine) : base(name, stateMachine) { }

    /// <summary>
    /// Coroutine to wait for a specified amount of time playing an animation before switching to the next state.
    /// </summary>
    protected IEnumerator WaitAndSwitchState(float waitTime, AState<TController, TStateMachine> nextState, int animation, string animationName)
    {
        _coroutineStarted = true;

        _behaviourController.actionText.text = animationName + " for " + waitTime + " seconds...";

        _behaviourController.ChangeAnimationTo(animation);

        yield return new WaitForSeconds(waitTime);

        _stateMachine?.SwitchState(nextState);
        _behaviourController.actionText.text = "";
        _coroutineStarted = false;
    }
    /// <summary>
    /// Coroutine to wait for a random amount of time playing an animation before switching to the next state.
    /// </summary>
    protected IEnumerator WaitAndSwitchState(AState<TController, TStateMachine> nextState, int animation, string animationName)
    {
        int waitTime = Random.Range(5, 21);
        return WaitAndSwitchState(waitTime, nextState, animation, animationName);
    }
}
