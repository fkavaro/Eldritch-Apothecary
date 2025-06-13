using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Replenisher : AHumanoid<Replenisher>
{
    public enum Personality
    {
        NORMAL, // Normal speed and time replenishing each stand. 0.2 to stop idling
        LAZY, // Lower speed and more time replenishing each stand. 0.3 to stop idling
        ENERGISED // Higher speed and less time replenishing each stand. 0.1 to stop idling
    }

    #region PUBLIC PROPERTIES
    [Header("Personality Properties")]
    public Personality personality = Personality.NORMAL;
    [Tooltip("Seconds needed to take or replenish supplies"), Range(1, 5)]
    public int timeToReplenish = 2;
    [Tooltip("Value to consider any replenishing. Below will be idling"), Range(0f, 0.5f)]
    public float replenishThreshold = 0.1f;
    [Header("Supplies Properties")]
    [Tooltip("Amount of carried supplies"), Range(0, 100)]
    public int capacity = 100;
    public int currentAmount = 0;
    public int remainingAmount;
    [Header("Supply Canvas")]
    public Slider supplyBar;
    public TextMeshProUGUI valueText;

    #endregion

    #region UTILITY SYSTEM
    UtilitySystem<Replenisher> _replenisherUS;
    Replenish_ReplenisherAction replenishShopAction;
    Replenish_ReplenisherAction replenishAlchemistAction;
    Replenish_ReplenisherAction replenishSorcererAction;
    GoAfterShopLifter_ReplenisherAction goAfterShopLifter_ReplenisherAction;
    Idle_ReplenisherAction idleAction;
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
            true
        );

        replenishAlchemistAction = new(
            "Replenishing alchemist",
            _replenisherUS,
            ApothecaryManager.Instance.alchemistShelves,
            ApothecaryManager.Instance.staffSuppliesShelves
        );

        replenishSorcererAction = new(
            "Replenishing sorcerer",
            _replenisherUS,
            ApothecaryManager.Instance.sorcererShelves,
            ApothecaryManager.Instance.staffSuppliesShelves
        );

        idleAction = new(_replenisherUS);

        goAfterShopLifter_ReplenisherAction = new(_replenisherUS);

        return _replenisherUS;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        personality = (Personality)UnityEngine.Random.Range(0, 3); // Chooses a personality randomly

        switch (personality)
        {
            case Personality.NORMAL:
                speed = 3f;
                timeToReplenish = UnityEngine.Random.Range(2, 4);
                replenishThreshold = 0.2f;
                break;
            case Personality.LAZY:
                speed = 2f;
                timeToReplenish = UnityEngine.Random.Range(4, 6);
                replenishThreshold = 0.3f;
                break;
            case Personality.ENERGISED:
                speed = 4f;
                timeToReplenish = 1;
                replenishThreshold = 0.1f;
                break;
        }
    }

    protected override void OnStart()
    {
        base.OnStart();

        supplyBar.maxValue = capacity;

        // Set supply bar value to currenValue
        supplyBar.value = currentAmount;
        valueText.text = currentAmount.ToString();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        // Supply bar isn't updated
        if (supplyBar.value != currentAmount)
        {
            // Set supply bar value to currenValue
            supplyBar.value = currentAmount;
            valueText.text = currentAmount.ToString();
        }

        remainingAmount = capacity - currentAmount;
    }

    public override bool CatIsBothering()
    {
        return false; // Cat never bothers
    }
    #endregion


    public bool IsFull()
    {
        if (currentAmount >= capacity)
        {
            currentAmount = capacity;
            return true;
        }
        else
            return false;
    }

    public bool IsEmpty()
    {
        if (currentAmount <= 0)
        {
            currentAmount = 0;
            return true;
        }
        else
            return false;
    }
}
