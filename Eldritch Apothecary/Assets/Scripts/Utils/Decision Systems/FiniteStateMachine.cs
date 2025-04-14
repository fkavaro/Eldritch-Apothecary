using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FiniteStateMachine : ADecisionSystem
{
    public FiniteStateMachine(ABehaviourController controller) : base(controller) { }

    /// <summary>
    /// The current active state of the state machine
    /// </summary>
    public AState currentState;

    protected AState initialState;

    protected override void Debug()
    {
        //if (controller.debugMode)
        //Debug.LogWarning(controller.transform.name + " is " + currentState.ToString());
        controller.stateText.text = currentState.stateName;
    }

    public void SetInitialState(AState state)
    {
        if (state == currentState) return;

        initialState = state;
        currentState = state;
        Debug();
        currentState.StartState();
    }

    /// <summary>
    /// Switchs to another state after exiting the current.
    /// </summary>
    public virtual void SwitchState(AState state)
    {
        if (state == currentState) return;

        currentState.ExitState();
        currentState = state;
        Debug();
        currentState.StartState();
    }

    /// <summary>
    /// Switchs back to initial state
    /// </summary>
    public override void Reset()
    {
        SwitchState(initialState);
    }

    #region UNITY EXECUTION EVENTS
    public override void Awake()
    {
        //currentState?.AwakeState(); NO
    }

    public override void Start()
    {
        //currentState?.StartState(); NO
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
