using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AStateMachine<TController, TStateMachineType> : ADecisionSystem<TController>
where TController : ABehaviourController<TController>
where TStateMachineType : AStateMachine<TController, TStateMachineType>
{
    protected AState<TController, TStateMachineType> currentState, initialState;

    protected AStateMachine(TController controller) : base(controller) { }

    #region TO BE IMPLEMENTED METHODS
    public abstract void SetInitialState(AState<TController, TStateMachineType> state);
    public abstract void SwitchState(AState<TController, TStateMachineType> state);
    #endregion

    #region INHERITED METHODS
    /// <summary>
    /// Switchs back to initial state
    /// </summary>
    public override void Reset()
    {
        SwitchState(initialState);
    }

    /// <summary>
    /// Debugs the current state of the state machine.
    /// </summary>
    protected override void DebugDecision()
    {
        //if (controller.debugMode)
        //Debug.LogWarning(controller.transform.name + " is " + currentState.ToString());
        controller.stateText.text = currentState.StateName;
    }
    #endregion

    #region UNITY EXECUTION EVENTS
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
