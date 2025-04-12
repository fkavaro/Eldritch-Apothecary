using System;
using UnityEngine;

public class Receptionist : AHumanoid
{
    #region PUBLIC PROPERTIES
    #endregion

    #region PRIVATE PROPERTIES
    StackFiniteStateMachine receptionistSFSM;
    #endregion

    #region STATES
    public Idle_ReceptionistState idleState;
    #endregion

    protected override ADecisionSystem CreateDecisionSystem()
    {
        // Stack Finite State Machine
        receptionistSFSM = new(this);

        // States initialization
        idleState = new(receptionistSFSM, this);

        // Initial state
        receptionistSFSM.SetInitialState(idleState);

        return receptionistSFSM;
    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {

    }

    #region PUBLIC METHODS
    #endregion

    #region PRIVATE METHODS
    #endregion
}