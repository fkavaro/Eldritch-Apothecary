using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Replenisher : AHumanoid<Replenisher>
{
    #region PUBLIC PROPERTIES
    [Header("Replenisher Properties")]
    [Tooltip("Amount of supplies carrying"), Range(0, 100)]
    public int capacity = 100;
    public int currentAmount = 0;
    public int remainingAmount;
    public Slider supplyBar;
    public Image fill;
    public TextMeshProUGUI valueText;

    #endregion

    #region PRIVATE PROPERTIES
    UtilitySystem<Replenisher> _replenisherUS;
    #endregion

    #region ACTIONS
    Replenish_ReplenisherAction replenishShopAction;
    Replenish_ReplenisherAction replenishAlchemistAction;
    Replenish_ReplenisherAction replenishSorcererAction;
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
            ApothecaryManager.Instance.shopSuppliesShelves
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

        return _replenisherUS;
    }

    protected override void OnStart()
    {
        supplyBar.maxValue = capacity;

        // Set supply bar value to currenValue
        supplyBar.value = currentAmount;
        valueText.text = currentAmount.ToString();
    }

    protected override void OnUpdate()
    {
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
