using System.Collections;
using UnityEngine;

public class WaitingForFreeSpace_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public WaitingForFreeSpace_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Waiting For Free Space", stackFsm) { }

    Potion emptySpot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        // Go to alchemist Table
        _controller.ChangeAnimationTo(_controller.idleAnim);
    }

    public override void UpdateState()
    {
        /*//Añadir probabilidad spawn de charco que se pare, aniamcion yell y spawn charco, material reflectante, en la posicion del controller, ir a picking up, prefab co ntrigger si lo toca el gato que se suscriba al evento que se cambie el color del gatro
        if (_controller.IsCloseToDestination(1))
        {//IsClose(Spot de las pociones)
         // Asignar el turno actual del alquimista a ese spot
            WaitUntilSlotAvailable();
            emptySpot.Assign(ApothecaryManager.Instance.currentAlchemistTurn);
            ApothecaryManager.Instance.NextAlchemistTurn();
            SwitchStateAfterCertainTime(1f, _controller.waitingState, _controller.pickUpAnim, "Placing potion");
        }*/
        SwitchStateAfterCertainTime(2f, _controller.finishingPotionState, _controller.pickUpAnim, "Finishing Potions");

    }
    private IEnumerator WaitUntilSlotAvailable()
    {
        emptySpot = ApothecaryManager.Instance.RandomPreparedPotion(false);

        while (emptySpot == null)
        {
            emptySpot = ApothecaryManager.Instance.RandomPreparedPotion(false);

            yield return new WaitForSeconds(0.5f);
        }

    }
}
