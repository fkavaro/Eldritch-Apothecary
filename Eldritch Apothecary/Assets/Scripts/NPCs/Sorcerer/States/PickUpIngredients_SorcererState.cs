using UnityEngine;

/// <summary>
/// Picks up an ingredient from a random shelf
/// </summary>
public class PickUpIngredients_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    Shelf _shelf;
    int amountNeeded;

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
                amountNeeded = Random.Range(20, 40);
                break;

            case Sorcerer.Efficiency.NORMAL:
                amountNeeded = Random.Range(10, 20);
                break;

            case Sorcerer.Efficiency.EFFICIENT:
                amountNeeded = Random.Range(5, 10);
                break;
        }
    }

    public override void UpdateState()
    {
        // Has reached exact position
        if (_controller.HasArrivedAtDestination())
        {
            if (_shelf.CanTake(amountNeeded))
            {
                _shelf.Take(amountNeeded);
                SwitchStateAfterCertainTime(1f, _controller.attendingClientsState, _controller.pickUpAnim, "Picking up ingredient");
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
            // Pick up position is occupied
            if (_controller.DestinationSpotIsOccupied())
            {
                // Stop and wait
                _controller.SetIfStopped(true);
                _controller.ChangeAnimationTo(_controller.waitAnim);
            }
            else // Pick up position is free
            {
                _controller.SetIfStopped(false);
            }
        }
    }
}
