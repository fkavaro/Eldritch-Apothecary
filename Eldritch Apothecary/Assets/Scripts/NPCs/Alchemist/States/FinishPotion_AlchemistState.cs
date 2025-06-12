using System.Collections;
using UnityEngine;

public class FinishPotion_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    //Empty spot where the potion is going to be placed
    Potion emptySpot;
    GameObject puddle;

    public FinishPotion_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Finish potion", stackFsm) { }

    public override void StartState()
    {
        puddle = ApothecaryManager.Instance.puddlePrefab;

        // Generates a random empty spot where the potion can be placed
        emptySpot = ApothecaryManager.Instance.RandomPreparedPotion(false);
        // If there is enough free space
        if (emptySpot != null)
        {
            // Goes to tha spot
            _controller.SetDestination(emptySpot.transform.position);
        }
        else
        {
            //If there isn't enough space to place the potion, changes to waiting for free space state
            SwitchStateAfterCertainTime(1f, _controller.waitingForSpaceState, _controller.idleAnim, "Waiting for free space");

        }
    }

    public override void UpdateState()
    {
        if (_controller.IsCloseToDestination(1))
        {
            // Checks if the potion has to fall
            if (UnityEngine.Random.Range(0, 10) < _controller.failProbability)
            {
                // Spawns a puddle on the alchemist's position
                GameObject.Instantiate(puddle, _controller.transform.position, _controller.transform.rotation);
                // Goes to pick ingredients again
                SwitchStateAfterCertainTime(1f, _controller.pickingUpIngredientsState, _controller.yellAnim, "Prepare potion again");

            }
            else
            {
                // If the potion doesn't fall, he assing to the empty spot a potion with the number of the current client
                emptySpot.Assign(ApothecaryManager.Instance.currentAlchemistTurn);
                // After 1 second placing the potion, changes to waiting state
                SwitchStateAfterCertainTime(1f, _controller.waitingState, _controller.pickUpAnim, "Placing potion");
            }
        }
    }


}
