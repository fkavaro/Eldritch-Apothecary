using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Replenish_ReplenisherAction : ALinearAction<Replenisher>
{
    /// <summary>
    /// List of shelves to be replenished
    /// </summary>
    List<Shelf> _consumedShelves = new();
    Shelf _consumedShelf;

    /// <summary>
    /// List of shelves from which supplies are being taken
    /// </summary>
    List<Shelf> _supplyShelves = new();
    Shelf _supplyShelf;

    /// <summary>
    /// Amount of supplies lacking
    /// </summary>
    int _lackingAmount;
    float _normalisedLackingAmount;

    bool _hasTakenAllSupplies = false;

    public Replenish_ReplenisherAction(string name, UtilitySystem<Replenisher> utilitySystem, List<Shelf> consumedShelves, List<Shelf> supplyShelves, int lack, float normalisedLack)
    : base(name, utilitySystem)
    {
        _consumedShelves = consumedShelves;
        _supplyShelves = supplyShelves;
        _lackingAmount = lack;
        _normalisedLackingAmount = normalisedLack;
    }

    protected override float SetDecisionFactor()
    {
        // Normalized value of lacking supplies respect to the total amount that can be stored
        return _normalisedLackingAmount;
    }

    public override void StartAction()
    {
        _supplyShelf = ApothecaryManager.Instance.RandomShelf(_supplyShelves);
    }

    public override void UpdateAction()
    {
        if (_supplyShelf == null) return;

        // Replenisher carries all lacking supplies
        if (_hasTakenAllSupplies)
        {
            // Order list incrementally from smallest amount to bigges amount
            _consumedShelves.Sort((a, b) => b.Amount - a.Amount);

            // Look for a shelf with lacking supplies
            foreach (Shelf shelf in _consumedShelves)
            {
                if (!shelf.IsFull())
                {
                    _consumedShelf = shelf;
                    break;
                }
            }

            // Has arrived to that shelf with lacking supplies
            if (_controller.HasArrived(_consumedShelf.transform.position))
                // Rplenish it, reducing carried amount
                _controller.carriedSuppliesAmount = _consumedShelf.Replenish(_controller.carriedSuppliesAmount);
            // Hasn't arrived to that shelf
            else
                // Keep it as destination
                _controller.SetDestinationSpot(_consumedShelf);

        }
        // Replenisher can carry more lacking suppplies
        else
        {
            // Has arrived to supplies shelf
            if (_controller.HasArrived(_supplyShelf.transform.position))
            {
                // Calculate remaining amount that can be carried
                int remainingAmountToCarry = 100 - _controller.carriedSuppliesAmount;
                int amountToTake;

                // Is greater than 10
                if (remainingAmountToCarry >= 10)
                    // Take random amount of supplies
                    amountToTake = UnityEngine.Random.Range(10, remainingAmountToCarry + 1);
                else
                    // Take remaining amount
                    amountToTake = remainingAmountToCarry;

                // Carrying more supplies
                _controller.carriedSuppliesAmount += amountToTake;

                // Can't take anymore supplies
                if (_controller.carriedSuppliesAmount >= 100 || _controller.carriedSuppliesAmount >= _lackingAmount)
                {
                    _hasTakenAllSupplies = true;

                    // Fix carried amount
                    if (_controller.carriedSuppliesAmount > 100)
                        _controller.carriedSuppliesAmount = 100;
                    else if (_controller.carriedSuppliesAmount > _lackingAmount)
                        _controller.carriedSuppliesAmount = _lackingAmount;
                }
                // Replenisher still can carry more lacking supplies 
                else //if (_controller.carriedSuppliesAmount < 100 || _controller.carriedSuppliesAmount < _lackingAmount)
                    // Go to another supply shelf
                    _supplyShelf = ApothecaryManager.Instance.RandomShelf(_supplyShelves);
            }
            // Hasn't arrived to supplies shelf
            else
                // Keep it as destination
                _controller.SetDestinationSpot(_supplyShelf);
        }

    }

    public override bool IsFinished()
    {
        // True if all supplies lacking were replenished
        return _hasTakenAllSupplies && _controller.carriedSuppliesAmount <= 0;
    }
}