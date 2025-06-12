using System.Collections;
using UnityEngine;

public class FinishPotion_AlchemistState : ANPCState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    Potion emptySpot;
    private bool waitingForSlot = false;

    public FinishPotion_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Finish potion", stackFsm) { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
        emptySpot = ApothecaryManager.Instance.RandomPreparedPotion(false);
        if (emptySpot != null)
        {
            _controller.SetDestination(emptySpot.transform.position);
        }
        else
        {
            //cambiar a esperar hueco (HACER ESTADO)
            SwitchStateAfterCertainTime(1f, _controller.waitingForSpaceState, _controller.idleAnim, "Waiting for free space");

        }
    }

    public override void UpdateState()
    {
        //Añadir probabilidad spawn de charco que se pare, aniamcion yell y spawn charco, material reflectante, en la posicion del controller, ir a picking up, prefab co ntrigger si lo toca el gato que se suscriba al evento que se cambie el color del gatro
        if (_controller.IsCloseToDestination(1))
        {//IsClose(Spot de las pociones)
         // Asignar el turno actual del alquimista a ese spot
            if (UnityEngine.Random.Range(0, 10) < _controller.failProbability)
            {
                //Se spawnea el charco
                GameObject.Instantiate(_controller.puddle, _controller.transform.position, _controller.transform.rotation);
                SwitchStateAfterCertainTime(1f, _controller.preparingPotionState, _controller.yellAnim, "Prepare potion again");

            }
            else
            {
                /* while (ApothecaryManager.Instance._turnleftPotions.Contains(ApothecaryManager.Instance.currentAlchemistTurn))
                 {
                     Debug.Log("No está!!");
                     ApothecaryManager.Instance.NextAlchemistTurn();
                 }*/
                //ApothecaryManager.Instance.NextAlchemistTurn();

                emptySpot.Assign(ApothecaryManager.Instance.currentAlchemistTurn);
                //ApothecaryManager.Instance.NextAlchemistTurn();

                SwitchStateAfterCertainTime(1f, _controller.waitingState, _controller.pickUpAnim, "Placing potion");
            }
        }
    }
    private IEnumerator WaitUntilSlotAvailable()
    {
        float maxWaitTime = 10f;
        float waited = 0f;

        while (ApothecaryManager.Instance.RandomPreparedPotion(false) == null)
        {
            yield return new WaitForSeconds(0.5f);
            waited += 0.5f;

            if (waited >= maxWaitTime)
            {
                Debug.LogWarning("Se agotó el tiempo de espera para encontrar hueco de poción.");
                SwitchState(_controller.waitingState);
                yield break;
            }
        }

        // Hueco disponible → volver a intentar
        waitingForSlot = false;
    }

}
