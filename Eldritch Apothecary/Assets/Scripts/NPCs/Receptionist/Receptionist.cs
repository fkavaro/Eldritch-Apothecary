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
    UtilitySystem<Receptionist> _receptionistUS;
    #endregion

    #region ACTIONS
    public Idle_ReceptionistAction idleAction;
    public Serving_ReceptionistAction servingAction;
    public Attending_ReceptionistAction attendingAction;
    public CalmingDown_ReceptionistAction calmingDownAction;
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Receptionist> CreateDecisionSystem()
    {
        // Utility System
        _receptionistUS = new(this);

        // Actions initialization
        idleAction = new(_receptionistUS, true);// Default action: will try to make decisions
                                                //_receptionistUS.SetDefaultAction(idleAction);
        servingAction = new(_receptionistUS);
        attendingAction = new(_receptionistUS);
        calmingDownAction = new(_receptionistUS);

        return _receptionistUS;
    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {

    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Returns true if there is a client to attend or potion to serve.
    /// </summary>
    public bool IsBusy()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region PRIVATE METHODS
    #endregion
}