using System;
using System.Collections.Generic;
using UnityEngine;

public class ResupplyStrategy : AStrategy<Replenisher>
{
    List<Shelf> _shelvesToResupply;
    Shelf _nextShelf;

    bool _hasResupplied = false;

    public ResupplyStrategy(Replenisher controller, List<Shelf> shelvesToResupply) : base(controller)
    {
        _shelvesToResupply = shelvesToResupply;

        // Finished taking a supply when coroutine finished
        _controller.CoroutineFinishedEvent += () => _hasResupplied = true;
    }

    public override Node<Replenisher>.Status Update()
    {
        if (_nextShelf == null)
            NextMostLackingShelf();

        // Has arrived to shelf to be resupplied
        if (_controller.HasArrivedAtDestination())
        {
            _controller.StartCoroutine(_controller.PlayAnimationCertainTime(2f, _controller.pickUpAnim, "Picking up supplies", false));

            // Coroutine has finished
            if (_hasResupplied)
            {
                // Replenish it, reducing carried amount
                _controller.currentAmount = _nextShelf.Replenish(_controller.currentAmount);

                // Isn't carrying anymore supplies
                if (_controller.IsEmpty())
                    return Node<Replenisher>.Status.Success;
                else
                {
                    NextMostLackingShelf();
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

    private void NextMostLackingShelf()
    {
        // Order list incrementally from smallest amount to biggest amount
        _shelvesToResupply.Sort((a, b) => b.Amount - a.Amount);

        // Look for a shelf with lacking supplies
        foreach (Shelf shelf in _shelvesToResupply)
        {
            if (!shelf.IsFull())
            {
                _nextShelf = shelf;
                break;
            }
        }

        _controller.SetDestinationSpot(_nextShelf);
    }
}
