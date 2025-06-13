using System;
using System.Collections.Generic;
using UnityEngine;

public class TakeSuppliesStrategy : AStrategy<Replenisher>
{
    List<Shelf> _supplyShelves;
    Shelf _nextShelf;
    int _lackingAmount;

    public TakeSuppliesStrategy(Replenisher controller, List<Shelf> supplyShelves, int lackingAmount) : base(controller)
    {
        _supplyShelves = supplyShelves;
        _lackingAmount = lackingAmount;
    }

    public override Node<Replenisher>.Status Update()
    {
        // Can't take anymore supplies
        if (_controller.IsFull() || _controller.currentAmount >= _lackingAmount)
            return Node<Replenisher>.Status.Success;

        if (_nextShelf == null)
            NextRandomSupplyShelf();

        // Has arrived to supplies shelf
        if (_controller.HasArrivedAtDestination())
        {
            _controller.PlayAnimationCertainTime(_controller.timeToReplenish, _controller.pickUpAnim, "Taking supplies", TakeSupply, false);
            return Node<Replenisher>.Status.Running;
        }
        // Hasn't arrived to shelf
        else
        {
            return Node<Replenisher>.Status.Running;
        }
    }

    void TakeSupply()
    {
        int amountToTake;

        // Is greater than 10
        if (_controller.remainingAmount >= 10)
            // Take random amount of supplies
            amountToTake = UnityEngine.Random.Range(10, 51);
        else
            // Take remaining amount
            amountToTake = _controller.remainingAmount;

        // Carrying more supplies
        _controller.currentAmount += amountToTake;

        // Can't take anymore supplies
        if (_controller.IsFull())
        {
            // Fix carried amount
            _controller.currentAmount = 100;
        }
        else if (_controller.currentAmount > _lackingAmount)
        {
            // Fix carried amount
            _controller.currentAmount = _lackingAmount;
        }

        _nextShelf = null;
    }

    private void NextRandomSupplyShelf()
    {
        _nextShelf = ApothecaryManager.Instance.RandomShelf(_supplyShelves);

        if (_nextShelf == null && _controller.debugMode)
            Debug.LogWarning("Shelf to replenish not found");
        else
            _controller.SetDestinationSpot(_nextShelf);
    }
}
