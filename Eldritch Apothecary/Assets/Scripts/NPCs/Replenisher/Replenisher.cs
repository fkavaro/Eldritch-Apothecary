using System;
using UnityEngine;

public class Replenisher : AHumanoid<Replenisher>
{
    #region PUBLIC PROPERTIES
    [Header("Replenisher Properties")]
    [Tooltip("Amount of supplies carrying"), Range(0, 100)]
    public int carriedSuppliesAmount = 0;
    #endregion

    #region PRIVATE PROPERTIES
    UtilitySystem<Replenisher> _replenisherUS;
    #endregion

    #region ACTIONS
    Replenish_ReplenisherAction replenishShopAction;
    Replenish_ReplenisherAction replenishAlchemistAction;
    Replenish_ReplenisherAction replenishSorcererAction;
    Idle_ReplenisherAction idleAction;
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
        // Utility System
        _replenisherUS = new(this);

        // Actions initialization
        replenishShopAction = new(
            "Replenishing shop",
            _replenisherUS,
            ApothecaryManager.Instance.shopShelves,
            ApothecaryManager.Instance.shopSuppliesShelves,
            ApothecaryManager.Instance.shopLack,
            ApothecaryManager.Instance.normalisedShopLack
        );

        replenishAlchemistAction = new(
            "Replenishing alchemist",
            _replenisherUS,
            ApothecaryManager.Instance.alchemistShelves,
            ApothecaryManager.Instance.staffSuppliesShelves,
            ApothecaryManager.Instance.alchemistLack,
            ApothecaryManager.Instance.normalisedAlchemistLack
        );

        replenishSorcererAction = new(
            "Replenishing sorcerer",
            _replenisherUS,
            ApothecaryManager.Instance.sorcererShelves,
            ApothecaryManager.Instance.staffSuppliesShelves,
            ApothecaryManager.Instance.sorcererLack,
            ApothecaryManager.Instance.normalisedSorcererLack
        );

        idleAction = new(_replenisherUS);

        return _replenisherUS;
    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {

    }

    public override bool CatIsBothering()
    {
        return false; // Cat never bothers
    }
    #endregion
}
