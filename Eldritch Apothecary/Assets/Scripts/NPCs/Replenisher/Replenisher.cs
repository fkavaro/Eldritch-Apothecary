using System;
using UnityEngine;

public class Replenisher : AHumanoid<Replenisher>
{
    #region PUBLIC PROPERTIES
    [Header("Replenisher Properties")]
    [Tooltip("Amount of supplies carrying"), Range(0, 100)]
    public int carriedSuppliesAmount = 0;
    // Shop shelves lacking supplies dictionary (shelf, lacking amount)
    // Staff shelves lacking supplies dictionary (shelf, lacking amount)
    #endregion

    #region PRIVATE PROPERTIES

    #endregion

    #region ACTIONS
    // Look for lacking shop supplies (each shelf added to dictionary):
    // - Walk around the store and ask blackboard shop shelves lacking supplies

    // Take shop supplies from random shelves (until carrying amount needed or max 100)

    // Replenish shop shelves from dictionary, reducing carried supplies amount

    // Look for lacking staff supplies (each shelf added to dictionary):
    //  - Walk to alchimist and sorcerer rooms and ask blackboard shelves lacking supplies

    // Take staff supplies from random shelves (until carrying amount needed or max 100)

    // Replenish staff shelves from dictionary, reducing carried supplies amount

    #endregion


    #region INHERITED METHODS
    protected override ADecisionSystem<Replenisher> CreateDecisionSystem()
    {
        throw new NotImplementedException();
    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {

    }

    public override bool CatIsBothering()
    {
        return false;
    }
    #endregion
}
