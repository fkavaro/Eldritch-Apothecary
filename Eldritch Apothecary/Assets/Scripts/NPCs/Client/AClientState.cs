using UnityEngine;
using System.Collections;

public abstract class AClientState : AState
{
    /// <summary>
    /// The referenced Client gameobject.
    /// </summary>
    protected Client clientContext;

    /// <summary>
    /// Flag to check if the coroutine has started.
    /// </summary>
    protected bool coroutineStarted = false;

    public AClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm)
    {
        this.clientContext = clientContext;
    }
    /// <summary>
    /// Coroutine to wait for a specified amount of time before switching to the next state.
    /// </summary>
    protected IEnumerator WaitAndSwitchState(float waitTime, AClientState nextState)
    {
        coroutineStarted = true;
        Debug.Log("Waiting for " + waitTime + " seconds...");
        yield return new WaitForSeconds(waitTime);
        stackFsm.SwitchState(nextState);
        coroutineStarted = false;
    }
    /// <summary>
    /// Coroutine to wait for a random amount of time before switching to the next state.
    /// </summary>
    protected IEnumerator WaitAndSwitchState(AClientState nextState)
    {
        int waitTime = Random.Range(5, 21);
        coroutineStarted = true;
        Debug.Log("Waiting for " + waitTime + " seconds...");
        yield return new WaitForSeconds(waitTime);
        stackFsm.SwitchState(nextState);
        coroutineStarted = false;
    }
}
