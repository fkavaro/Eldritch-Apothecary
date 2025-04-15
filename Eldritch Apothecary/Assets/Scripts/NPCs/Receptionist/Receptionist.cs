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

    internal bool IsBusy()
    {
        throw new NotImplementedException();
    }

    #region PUBLIC METHODS
    #endregion

    #region PRIVATE METHODS
    #endregion
}