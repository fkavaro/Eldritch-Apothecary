using System;
using UnityEngine;

public class Receptionist : AHumanoid<Receptionist>
{
    #region UTILITY SYSTEM
    UtilitySystem<Receptionist> _receptionistUS;
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
        _idleAction = new(_receptionistUS);
        _servingAction = new(_receptionistUS);
        _attendingAction = new(_receptionistUS);
        _calmingDownAction = new(_receptionistUS);

        return _receptionistUS;
    }

    public override bool CatIsBothering()
    {
        // Cat never bothers the receptionist
        return false;
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
    public bool CanAttend()
    {
        return _receptionistUS.IsCurrentAction(_attendingAction)
        && HasArrived(ApothecaryManager.Instance.receptionistAttendingSpot.transform.position);
    }

    /// <summary>
    /// Returns true if the receptionist is ready to attend clients complaining
    /// </summary>
    internal bool CanCalmDown()
    {
        return _receptionistUS.IsCurrentAction(_calmingDownAction)
        && HasArrived(ApothecaryManager.Instance.receptionistCalmDownSpot.transform.position);
    }
    #endregion
}