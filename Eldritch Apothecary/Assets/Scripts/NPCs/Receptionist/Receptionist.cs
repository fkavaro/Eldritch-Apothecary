using System;
using UnityEngine;

/// <summary>
/// Class representing the receptionist NPC in the game.
/// </summary>
public class Receptionist : AHumanoid<Receptionist>
{
    #region PUBLIC PROPERTIES
    [Header("Receptionist Properties")]
    #endregion

    #region PRIVATE PROPERTIES
    UtilitySystem<Receptionist> _receptionistUS;
    #endregion

    #region ACTIONS
    Idle_ReceptionistAction _idleAction;
    Serving_ReceptionistAction _servingAction;
    Attending_ReceptionistAction _attendingAction;
    CalmingDown_ReceptionistAction _calmingDownAction;
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Receptionist> CreateDecisionSystem()
    {
        // Utility System
        _receptionistUS = new(this);

        // Actions initialization
        _idleAction = new(_receptionistUS, true);// Default action: will try to make decisions
                                                 //_receptionistUS.SetDefaultAction(idleAction);
        _servingAction = new(_receptionistUS);
        _attendingAction = new(_receptionistUS);
        _calmingDownAction = new(_receptionistUS);

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
        return ApothecaryManager.Instance.waitingQueue.HasAnyClient();// || ApothecaryManager.Instance.potionManager.HasPotionToServe();
    }

    /// <summary>
    /// Returns true if the receptionist is ready to attend clients at the counter.
    /// </summary>
    /// <returns></returns>
    public bool Interact()
    {
        return _receptionistUS.IsCurrentAction(_attendingAction);
    }
    #endregion

    #region PRIVATE METHODS
    #endregion
}