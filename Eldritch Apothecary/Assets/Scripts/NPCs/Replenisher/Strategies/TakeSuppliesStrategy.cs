using System;
using System.Collections.Generic;
using UnityEngine;

public class TakeSuppliesStrategy : AStrategy<Replenisher>
{
    List<Shelf> _supplyShelves;
    Shelf _nextShelf;
    int _lackingAmount;
    bool _hasTakenSupply = false;

    public TakeSuppliesStrategy(Replenisher controller, List<Shelf> supplyShelves, int lackingAmount) : base(controller)
    {
        _supplyShelves = supplyShelves;
        _lackingAmount = lackingAmount;

        // Finished taking a supply when coroutine finished
        _controller.CoroutineFinishedEvent += () => _hasTakenSupply = true;
    }

    public override Node<Replenisher>.Status Update()
    {
        if (_nextShelf == null)
            NextRandomSupplyShelf();

        // Has arrived to supplies shelf
        if (_controller.HasArrivedAtDestination())
        {
            _controller.StartCoroutine(_controller.PlayAnimationCertainTime(2f, _controller.pickUpAnim, "Picking up supplies", false));

            // Coroutine has finished
            if (_hasTakenSupply)
            {
                int amountToTake;

                // Is greater than 10
                if (_controller.remainingAmount >= 10)
                    // Take random amount of supplies
                    amountToTake = UnityEngine.Random.Range(10, _controller.remainingAmount + 1);
                else
                    // Take remaining amount
                    amountToTake = _controller.remainingAmount;

                // Carrying more supplies
                _controller.currentAmount += amountToTake;

                // Can't take anymore supplies
                if (_controller.IsFull() || _controller.currentAmount >= _lackingAmount)
                {
                    // Fix carried amount
                    if (_controller.currentAmount > _lackingAmount)
                        _controller.currentAmount = _lackingAmount;

                    return Node<Replenisher>.Status.Success;
                }
                // Replenisher still can carry more lacking supplies 
                else
                {
                    // Go to another supply shelf
                    NextRandomSupplyShelf();
                    return Node<Replenisher>.Status.Running;
                }
            }
            // Coroutine hasn't finished
            else
                return Node<Replenisher>.Status.Running;
        }
        // Hasn't arrived to shelf
        else
            return Node<Replenisher>.Status.Running;

    }

    private void NextRandomSupplyShelf()
    {
        _nextShelf = ApothecaryManager.Instance.RandomShelf(_supplyShelves);
        _controller.SetDestinationSpot(_nextShelf);
        _hasTakenSupply = false;
    }
}
