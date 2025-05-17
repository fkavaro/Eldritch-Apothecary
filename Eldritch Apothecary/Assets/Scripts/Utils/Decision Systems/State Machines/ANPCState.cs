using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Base class for NPC states, allowing to handle animations.
/// </summary>
public abstract class ANPCState<TController, TStateMachine> : AState<TController, TStateMachine>
    where TController : ANPC<TController>
    where TStateMachine : AStateMachine<TController, TStateMachine>
{
    public ANPCState(string name, TStateMachine stateMachine) : base(name, stateMachine) { }

    /// <summary>
    /// Coroutine to wait for a random amount of time playing an animation before switching to the next state.
    /// </summary>
    protected void SwitchStateAfterRandomTime(AState<TController, TStateMachine> nextState, int animation, string animationName)
    {
        if (!_controller.coroutineStarted)
        {
            int waitTime = Random.Range(5, 21);
            SwitchStateAfterCertainTime(waitTime, nextState, animation, animationName);
        }
    }

    protected void SwitchStateAfterCertainTime(float waitTime, AState<TController, TStateMachine> nextState, int animation, string animationName)
    {
        if (!_controller.coroutineStarted)
        {
            _controller.StartCoroutine(SwitchStateAfterCertainTimeCoroutine(waitTime, nextState, animation, animationName));
        }
    }

    /// <summary>
    /// Coroutine to wait for a specified amount of time playing an animation before switching to the next state.
    /// </summary>
    protected IEnumerator SwitchStateAfterCertainTimeCoroutine(float waitTime, AState<TController, TStateMachine> nextState, int animation, string animationName)
    {
        yield return _controller.PlayAnimationCertainTime(waitTime, animation, animationName);

        SwitchState(nextState);
    }

    /// <summary>
    /// Switches to the next state if the animation is finished.
    /// </summary>
    protected void SwitchStateAfterAnimation(AState<TController, TStateMachine> nextState)
    {
        if (_controller.IsAnimationFinished())
            SwitchState(nextState);
    }

    protected void SwitchStateAfterAnimation(int animation, AState<TController, TStateMachine> nextState)
    {
        _controller.ChangeAnimationTo(animation);
        SwitchStateAfterAnimation(nextState);
    }
}
