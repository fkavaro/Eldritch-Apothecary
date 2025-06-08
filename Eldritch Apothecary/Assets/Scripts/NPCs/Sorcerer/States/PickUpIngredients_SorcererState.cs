using UnityEngine;

public class PickUpIngredients_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    Shelf shelf;

    public PickUpIngredients_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
        : base("Picking up ingredient", sfsm) { }

    public override void StartState()
    {
        shelf = ApothecaryManager.Instance.RandomSorcererShelf();
        _controller.SetDestinationSpot(shelf);
    }

    public override void UpdateState()
    {
        // If is close to the pick up position
        if (_controller.IsCloseToDestination())
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

                // Has reached exact position
                if (_controller.HasArrivedAtDestination())
                {
                    // Picks up different amounts according to efficiency
                    switch (_controller.efficiency)
                    {
                        case Sorcerer.Efficiency.INEFFICIENT:
                            shelf.TakeRandom(40);
                            break;

                        case Sorcerer.Efficiency.NORMAL:
                            shelf.TakeRandom(20);
                            break;

                        case Sorcerer.Efficiency.EFFICIENT:
                            shelf.TakeRandom(10);
                            break;
                    }
                    SwitchStateAfterCertainTime(1f, _controller.attendingClientsState, _controller.pickUpAnim, "Picking up ingredient");
                }
            }
        }
    }
}
