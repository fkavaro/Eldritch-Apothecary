using UnityEngine;
using System.Collections;

public abstract class ANPCState<TController, TStateMachine> : AState<TController, TStateMachine>
    where TController : ANPC<TController>
    where TStateMachine : AStateMachine<TController, TStateMachine>
{
    public ANPCState(string name, TStateMachine stateMachine) : base(name, stateMachine) { }

    /// <summary>
    /// Coroutine to wait for a specified amount of time before switching to the next state.
    /// </summary>
    protected IEnumerator WaitAndSwitchState(float waitTime, AState<TController, TStateMachine> nextState, int animation = -1, string action = "Executing animation")
    {
        _coroutineStarted = true;

        _behaviourController.actionText.text = action + " for " + waitTime + " seconds...";

        if (animation != -1)
        {
            _behaviourController.ChangeAnimationTo(animation);
        }

        yield return new WaitForSeconds(waitTime);

        _stateMachine?.SwitchState(nextState);
        _behaviourController.actionText.text = "";
        _coroutineStarted = false;
    }
    /// <summary>
    /// Coroutine to wait for a random amount of time before switching to the next state.
    /// </summary>
    protected IEnumerator WaitAndSwitchState(AState<TController, TStateMachine> nextState, int animation = -1, string action = "Acting")
    {
        int waitTime = Random.Range(5, 21);
        return WaitAndSwitchState(waitTime, nextState, animation, action);
    }
}
