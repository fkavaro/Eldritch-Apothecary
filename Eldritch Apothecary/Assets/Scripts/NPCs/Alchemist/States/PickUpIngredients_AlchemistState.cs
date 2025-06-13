using System.Collections.Generic;
using UnityEngine;

public class PickUpIngredients_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    int _amountNeeded, _takeAnotherIngredient;
    Shelf _shelf;
    List<Shelf> _visitedShelves = new();

    public PickUpIngredients_AlchemistState(StackFiniteStateMachine<Alchemist> sfsm)
        : base("Picking up ingredient", sfsm) { }

    public override void StartState()
    {
        _visitedShelves = new();

        // Number of Ingredients plus the extra ingredients defined by his personality
        _amountNeeded = UnityEngine.Random.Range(5, 20) + _controller.numExtraIngredients;

        _shelf = ApothecaryManager.Instance.RandomAlchemistShelf();

        _controller.SetDestinationSpot(_shelf);

        // Probability of taking another ingredient according to efficiency
        if (_controller.efficiency == Alchemist.Efficiency.INEFFICIENT
            || _controller.skill == Alchemist.Skill.NOOB)
            _takeAnotherIngredient = 5; // More probably if is inefficient
        else
            _takeAnotherIngredient = 2;
    }

    public override void UpdateState()
    {
        if (_controller.HasArrivedAtDestination())
        {
            // Needed amount can and has been taken
            if (_shelf.Take(_amountNeeded))
            {
                // Takes ingredient and goes to another shelf
                if (Random.Range(0, 11) <= _takeAnotherIngredient)
                    _controller.PlayAnimationCertainTime(2f, _controller.pickUpAnim, "Picking up ingredient", GoToOtherShelf, false);
                // Prepares potion
                else
                    SwitchStateAfterCertainTime(2f, _controller.preparingPotionState, _controller.pickUpAnim, "Picking up ingredient");
            }
            // Not enough ingredients
            else
            {
                _controller.ChangeAnimationTo(_controller.waitAnim);
                _controller.animationText.text = "Waiting for replenishment";
            }
        }
        // Is close to shelf
        else if (_controller.IsCloseToDestination())
        {
            // Shelf occupied or not enough supply for needed amount
            if (_controller.DestinationSpotIsOccupied() || !_shelf.CanTake(_amountNeeded))
            {
                // Stop and wait
                _controller.ChangeAnimationTo(_controller.waitAnim);
                _controller.SetIfStopped(true);

                // Not enough supply
                if (!_shelf.CanTake(_amountNeeded))
                    _controller.animationText.text = "Waiting for replenishment";
            }
            // Shelf is free and has enough supplies
            else if (!_controller.DestinationSpotIsOccupied() && _shelf.CanTake(_amountNeeded))
            {
                _controller.SetIfStopped(false);
                _controller.ChangeAnimationTo(_controller.walkAnim);
            }
        }
    }

    void GoToOtherShelf()
    {
        _amountNeeded = Random.Range(5, 16); // Random amount needed
        _shelf = ApothecaryManager.Instance.RandomShopShelf();
        while (_visitedShelves.Contains(_shelf)) // Check that it's new
            _shelf = ApothecaryManager.Instance.RandomShopShelf();
        _visitedShelves.Add(_shelf);
        _controller.SetDestinationSpot(_shelf);
        _controller.ChangeAnimationTo(_controller.walkAnim);
    }
}
