using UnityEngine;

public class PickUpIngredients_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    int numIngredients;
    public PickUpIngredients_AlchemistState(StackFiniteStateMachine<Alchemist> sfsm)
        : base("Picking up ingredient", sfsm) { }

    public override void StartState()
    {
        if (_controller.newShelf)
        {
            _controller.currentShelf = ApothecaryManager.Instance.RandomAlchemistShelf();
        }
        numIngredients = UnityEngine.Random.Range(5, 20) + _controller.numExtraIngredients;
        _controller.SetDestinationSpot(_controller.currentShelf);
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
                    if (_controller.currentShelf.CanTake(numIngredients))
                    {
                        _controller.currentShelf.Take(numIngredients);
                        if (UnityEngine.Random.Range(0, 10) < 5)
                        {
                            _controller.newShelf = true;
                            Debug.Log("Cogiendo Ingredientres");
                            SwitchStateAfterCertainTime(1f, _controller.preparingPotionState, _controller.pickUpAnim, "Picking up ingredient");
                        }
                        else
                        {
                            Debug.Log("cambio de estanteria");
                            _controller.currentShelf = ApothecaryManager.Instance.RandomAlchemistShelf();
                            _controller.SetDestinationSpot(_controller.currentShelf);
                            numIngredients = UnityEngine.Random.Range(5, 20);
                        }
                    }
                    else
                    {
                        Debug.Log("No hay ingredientes disponibles en esta estantería, esperando...");
                        SwitchStateAfterCertainTime(1f, _controller.waitingIngredientsState, _controller.waitAnim, "Picking up ingredient");
                    }
                }
            }
        }
    }
}
