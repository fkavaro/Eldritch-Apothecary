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
        // If he is close to the shelf
        if (_controller.IsCloseToDestination())
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
                            SwitchStateAfterCertainTime(1f, _controller.preparingPotionState, _controller.pickUpAnim, "Picking up ingredient");
                        }
                        else
                        {
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
                        // If the shelf doesnt have enough ingredients, changes to the waiting ingredients state
                        SwitchState(_controller.waitingIngredientsState);
                    }
                }
            }
        }
    }
}
