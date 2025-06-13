using UnityEngine;

public class PickUpIngredients_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    // Number of ingredients to pick from the shelf
    int numIngredients;
    // Saves last used shelf
    Shelf oldShelf;
    public PickUpIngredients_AlchemistState(StackFiniteStateMachine<Alchemist> sfsm)
        : base("Picking up ingredient", sfsm) { }

    public override void StartState()
    {
        // If he needs to change of shelf
        if (_controller.newShelf)
        {
            // Random Shelf calculated
            _controller.currentShelf = ApothecaryManager.Instance.RandomAlchemistShelf();
            // And it is saved
            oldShelf = _controller.currentShelf;
        }
        // Number of Ingredients plus the extra ingredients defined by his personality
        numIngredients = UnityEngine.Random.Range(5, 20) + _controller.numExtraIngredients;
        // Goes to the current shelf
        _controller.SetDestinationSpot(_controller.currentShelf);
    }

    public override void UpdateState()
    {
        
        if (_controller.HasArrivedAtDestination())
        {
            // Checks if there is enough ingredients in the shelf
            if (_controller.currentShelf.CanTake(numIngredients))
            {
                // Takes the number of ingredients from the shelf
                _controller.currentShelf.Take(numIngredients);
                // Probability of going to another shelf
                if (UnityEngine.Random.Range(0, 10) < 5)
                {
                    _controller.newShelf = true;
                    // Goes to prepare potion state
                    if (_controller.debugMode) Debug.Log("Hacer poción");
                    SwitchStateAfterCertainTime(1f, _controller.preparingPotionState, _controller.pickUpAnim, "Picking up ingredient");
                }
                else
                {
                    if (_controller.debugMode) Debug.Log("Cambio de estantería");

                    // Prevent checking the same shelf 2 times in a row
                    while (oldShelf == _controller.currentShelf)
                    {
                        _controller.currentShelf = ApothecaryManager.Instance.RandomAlchemistShelf();
                    }
                    _controller.SetDestinationSpot(_controller.currentShelf);
                    //Saves the new asignated shelf
                    oldShelf = _controller.currentShelf;
                    //New number of ingredients
                    numIngredients = UnityEngine.Random.Range(5, 20);
                    //SwitchState(_controller.pickingUpIngredientsState);
                }
            }
            else
            {
                // If the shelf doesnt have enough ingredients, waits
                if (_controller.debugMode) Debug.Log("Esperar ingredientes");
                _controller.ChangeAnimationTo(_controller.waitAnim);
                _controller.animationText.text = "Waiting for replenishment";
            }
        }
        else if (_controller.IsCloseToDestination()) // If he is close to the shelf
        {
            // Shelf occupied
            if (_controller.DestinationSpotIsOccupied())
            {
                // Changes to wait animation
                _controller.ChangeAnimationTo(_controller.waitAnim);
                // Stops the character
                _controller.SetIfStopped(true);

            }
            else
            {
                // Exit from being stopped
                _controller.SetIfStopped(false);
            }
        }
    }
}
