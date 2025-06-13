using System;
using System.Collections.Generic;
using UnityEngine;

public class ResupplyStrategy : AStrategy<Replenisher>
{
    List<Shelf> _shelvesToResupply;
    Shelf _nextShelf;

    public ResupplyStrategy(Replenisher controller, List<Shelf> shelvesToResupply) : base(controller)
    {
        _shelvesToResupply = shelvesToResupply;
    }

    public override Node<Replenisher>.Status Update()
    {
        // Isn't carrying anymore supplies or no more lacking supplies
        if (_controller.IsEmpty() || ApothecaryManager.Instance.GetNormalisedLack(_shelvesToResupply) <= 0)
            return Node<Replenisher>.Status.Success;

        // Don't know where to go
        if (_nextShelf == null)
        {
            NextMostLackingShelf();
            return Node<Replenisher>.Status.Running;
        }
        else
        {
            // Has arrived to shelf to be resupplied
            if (_controller.HasArrived(_nextShelf))
                _controller.PlayAnimationCertainTime(_controller.timeToReplenish, _controller.pickUpAnim, "Resuplying", Resupply, false);

            return Node<Replenisher>.Status.Running;
        }
    }

    void Resupply()
    {
        // Replenish it, reducing carried amount
        _controller.currentAmount = _nextShelf.Replenish(_controller.currentAmount);
        _nextShelf = null;
    }

    void NextMostLackingShelf()
    {
        // Order list incrementally from smallest amount to biggest amount
        _shelvesToResupply.Sort((a, b) => a.Amount - b.Amount);

        // Look for a shelf with lacking supplies
        foreach (Shelf shelf in _shelvesToResupply)
        {
            // Not full nor occupied
            if (!shelf.IsFull() && !shelf.IsOccupied())
            {
                _nextShelf = shelf;
                break;
            }
        }

        if (_nextShelf == null && _controller.debugMode)
            Debug.LogWarning("Shelf to resupply not found");
        else
            _controller.SetDestinationSpot(_nextShelf);
    }
}
