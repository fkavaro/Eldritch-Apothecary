using UnityEngine;

public class PickUpIngredients_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    Shelf shelf;
    int numIngredients;
    public PickUpIngredients_AlchemistState(StackFiniteStateMachine<Alchemist> sfsm)
        : base("Picking up ingredient", sfsm) { }

    public override void StartState()
    {
        shelf = ApothecaryManager.Instance.RandomAlchemistShelf();
        numIngredients = UnityEngine.Random.Range(5, 20);
        _controller.SetDestinationSpot(shelf);
    }

    public override void UpdateState()
    {
        if (_controller.IsCloseToDestination())
        {
            if (_controller.DestinationSpotIsOccupied())
            {
                _controller.SetIfStopped(true);
                _controller.ChangeAnimationTo(_controller.waitAnim);
            }
            else
            {
                _controller.SetIfStopped(false);

                if (_controller.HasArrivedAtDestination())
                {
                    // Verifica si puede tomar ingredientes
                    if (shelf.CanTake(numIngredients))
                    {
                        shelf.Take(numIngredients);
                        if (UnityEngine.Random.Range(0, 10) < 5)
                        {
                            SwitchStateAfterCertainTime(1f, _controller.preparingPotionState, _controller.pickUpAnim, "Picking up ingredient");
                        }
                        else
                        {
                            StartState();
                        }
                    }
                    else
                    {
                        Debug.Log("No hay ingredientes disponibles en esta estantería, esperando...");
                        SwitchState(_controller.waitingIngredientsState);
                    }
                }
            }
        }
    }
}
