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
    StackFiniteStateMachine<Receptionist> _receptionistSFSM;
    UtilitySystem<Receptionist> _receptionistUS;
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
        _receptionistSFSM = new(this);

        // States initialization
        idleState = new(_receptionistSFSM);

        // Initial state
        _receptionistSFSM.SetInitialState(idleState);

        return _receptionistSFSM;
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