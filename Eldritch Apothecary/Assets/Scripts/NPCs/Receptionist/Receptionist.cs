using System;
using UnityEngine;

public class Receptionist : AHumanoid<Receptionist>
{
    #region PUBLIC PROPERTIES
    public bool canCalmDown,
        canAttend,
        isBusy;
    #endregion

    #region UTILITY SYSTEM
    UtilitySystem<Receptionist> _receptionistUS;
    Idle_ReceptionistAction _idleAction;
    Serving_ReceptionistAction _servingAction;
    Attending_ReceptionistAction _attendingAction;
    CalmingDown_ReceptionistAction _calmingDownAction;
    Dump_ReceptionistAction _dumpAction;
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
        _dumpAction = new(_receptionistUS);

        return _receptionistUS;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        isBusy = ApothecaryManager.Instance.waitingQueue.HasAnyClient()
            || ApothecaryManager.Instance.IsSomeoneComplaining();

        canAttend = _receptionistUS.IsCurrentAction(_attendingAction)
            && HasArrived(ApothecaryManager.Instance.receptionistAttendingSpot);
    }

    public override bool CatIsBothering()
    {
        // Cat never bothers the receptionist
        return false;
    }
    #endregion
}