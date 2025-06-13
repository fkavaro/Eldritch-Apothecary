using UnityEngine;

/// <summary>
/// Picks up an ingredient from a random shelf
/// </summary>
public class PickUpIngredients_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    Shelf _shelf;
    int _amountNeeded;

    public PickUpIngredients_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
        : base("Picking up ingredient", sfsm) { }

    public override void StartState()
    {
        _shelf = ApothecaryManager.Instance.RandomSorcererShelf();
        _controller.SetDestinationSpot(_shelf);

        // Picks up different amounts according to efficiency
        switch (_controller.efficiency)
        {
            case Sorcerer.Efficiency.INEFFICIENT:
                _amountNeeded = Random.Range(15, 25);
                break;

            case Sorcerer.Efficiency.NORMAL:
                _amountNeeded = Random.Range(10, 15);
                break;

            case Sorcerer.Efficiency.EFFICIENT:
                _amountNeeded = Random.Range(5, 10);
                break;
        }
    }

    public override void UpdateState()
    {
        // Has reached exact position
        if (_controller.HasArrivedAtDestination())
        {
            if (_shelf.Take(_amountNeeded))
            {
                SwitchStateAfterCertainTime(2f, _controller.attendingClientsState, _controller.pickUpAnim, "Picking up ingredient");
            }
            else
            {
                _controller.ChangeAnimationTo(_controller.waitAnim);
                _controller.animationText.text = "Waiting for replenishment";
            }
        }
        // If is close to the pick up position
        else if (_controller.IsCloseToDestination())
        {
            // Pick up position is occupied or not enough supply for needed amount
            if (_controller.DestinationSpotIsOccupied() || !_shelf.CanTake(_amountNeeded))
            {
                // Stop and wait
                _controller.SetIfStopped(true);
                _controller.ChangeAnimationTo(_controller.waitAnim);

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
}
