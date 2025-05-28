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
        if (_nextShelf == null)
            NextMostLackingShelf();

        // Has arrived to shelf to be resupplied
        if (_controller.HasArrivedAtDestination())
        {
            _controller.PlayAnimationCertainTime(4f, _controller.pickUpAnim, "Resuplying", Resupply, false);

            // Isn't carrying anymore supplies or no more lacking supplies
            if (_controller.IsEmpty() || ApothecaryManager.Instance.GetNormalisedLack(_shelvesToResupply) <= 0)
                return Node<Replenisher>.Status.Success;
            else
                return Node<Replenisher>.Status.Running;
        }
        // Hasn't arrived to shelf
        else
            return Node<Replenisher>.Status.Running;

    }

    void Resupply()
    {
        // Replenish it, reducing carried amount
        _controller.currentAmount = _nextShelf.Replenish(_controller.currentAmount);

        // Is carrying more supplies
        if (!_controller.IsEmpty())
            NextMostLackingShelf();
    }

    private void NextMostLackingShelf()
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

        _controller.SetDestinationSpot(_nextShelf);
    }
}
