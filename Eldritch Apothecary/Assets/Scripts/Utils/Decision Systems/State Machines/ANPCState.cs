using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for NPC states, allowing to handle animations.
/// </summary>
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
        _controller.actionText.text = animationName + " for " + waitTime + " seconds...";

        _controller.ChangeAnimationTo(animation);

        return WaitAndSwitchState(waitTime, nextState);
    }
    /// <summary>
    /// Coroutine to wait for a random amount of time playing an animation before switching to the next state.
    /// </summary>
    protected IEnumerator RandomWaitAndSwitchState(AState<TController, TStateMachine> nextState, int animation, string animationName)
    {
        int waitTime = Random.Range(5, 21);
        return WaitAndSwitchState(waitTime, nextState, animation, animationName);
    }
}
