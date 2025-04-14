using System;
using UnityEngine;

/// <summary>
/// Class representing the receptionist NPC in the game.
/// </summary>
public class Receptionist : AHumanoid<Receptionist>
{
    #region PUBLIC PROPERTIES
    #endregion

    #region PRIVATE PROPERTIES
    StackFiniteStateMachine<Receptionist> receptionistSFSM;
    #endregion

    #region STATES
    public Idle_ReceptionistState idleState;
    public Serving_ReceptionistState servingState;
    public Attending_ReceptionistState attendingState;
    public CalmingDown_ReceptionistState calmingDownState;
    #endregion

    protected override ADecisionSystem<Receptionist> CreateDecisionSystem()
    {
        // Stack Finite State Machine
        receptionistSFSM = new(this);

        // States initialization
        idleState = new(receptionistSFSM);

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