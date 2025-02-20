using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : ADecisionSystem
{
    List<AState> states = new List<AState>();

    /// <summary>
    /// The current active state of the state machine
    /// </summary>
    public AState currentState
    {
        get
        {
            return currentState;
        }
        protected set
        {
            currentState = value;
            currentState.StartState();

            if (debug) Debug.LogWarning(currentState.ToString());
        }
    }

    public void CreateState(AState state)
    {
        states.Add(state);
    }

    public void SetInitialState(AState state)
    {
        currentState = state;
    }

    public void CreateTransition(AState from, AState to, bool condition)
    {
        from.AddTransition(to, condition);
    }

    /// <summary>
    /// Switchs to another state after exiting the current.
    /// </summary>
    public virtual void SwitchState(AState state)
    {
        currentState.ExitState();
        currentState = state;
        currentState.StartState();

        if (debug) Debug.LogWarning(currentState.ToString());
    }

    #region UNITY EXECUTION EVENTS
    public override void Awake()
    {
        currentState?.AwakeState();
    }

    public override void Start()
    {
        //currentState = SetInitialState();
        currentState?.StartState();
    }

    public override void Update()
    {
        currentState?.UpdateState();
    }
    #endregion

    # region COLLISION AND TRIGGER EVENTS
    public override void OnCollisionEnter(Collision collision)
    {
        currentState?.OnCollisionEnter(collision);
    }

    public override void OnCollisionStay(Collision collision)
    {
        currentState?.OnCollisionStay(collision);
    }

    public override void OnCollisionExit(Collision collision)
    {
        currentState?.OnCollisionExit(collision);
    }

    public override void OnTriggerEnter(Collider other)
    {
        currentState?.OnTriggerEnter(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        currentState?.OnTriggerStay(other);
    }

    public override void OnTriggerExit(Collider other)
    {
        currentState?.OnTriggerExit(other);
    }
    #endregion
}
