using System.Collections;
using UnityEngine;

public class FinishPotion_AlchemistState : AState<Alchemist, StackFiniteStateMachine<Alchemist>>
{
    public FinishPotion_AlchemistState(StackFiniteStateMachine<Alchemist> stackFsm)
    : base("Finish potion", stackFsm) { }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void StartState()
    {
<<<<<<< Updated upstream
        //Accion de terminar poci�n (3 segundos mas)
        _controller.StartCoroutine(FinishPotion());
=======
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
>>>>>>> Stashed changes
    }

    public override void UpdateState()
    {
<<<<<<< Updated upstream

=======
        //Añadir probabilidad spawn de charco que se pare, aniamcion yell y spawn charco, material reflectante, en la posicion del controller, ir a picking up, prefab co ntrigger si lo toca el gato que se suscriba al evento que se cambie el color del gatro
        if (_controller.IsCloseToDestination(1))
        {//IsClose(Spot de las pociones)
         // Asignar el turno actual del alquimista a ese spot
            if (UnityEngine.Random.Range(0, 10) < _controller.failProbability)
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
            {
                //Se spawnea el charco
                GameObject.Instantiate(_controller.puddle, _controller.transform.position, _controller.transform.rotation);
                GameObject.Destroy(_controller.puddle, 5f);
                SwitchStateAfterCertainTime(1f, _controller.preparingPotionState, _controller.yellAnim, "Prepare potion again");

            }
            else
            {
                emptySpot.Assign(ApothecaryManager.Instance.currentAlchemistTurn);
                ApothecaryManager.Instance.NextAlchemistTurn();
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
>>>>>>> Stashed changes
            {
                //Se spawnea el charco
                GameObject.Instantiate(_controller.puddle, _controller.transform.position, _controller.transform.rotation);
                GameObject.Destroy(_controller.puddle, 5f);
                SwitchStateAfterCertainTime(1f, _controller.preparingPotionState, _controller.yellAnim, "Prepare potion again");

            }
            else
            {
                emptySpot.Assign(ApothecaryManager.Instance.currentAlchemistTurn);
                ApothecaryManager.Instance.NextAlchemistTurn();
                SwitchStateAfterCertainTime(1f, _controller.waitingState, _controller.pickUpAnim, "Placing potion");
            }
        }
>>>>>>> Stashed changes
    }

    public override void ExitState()
    {
    }

    private IEnumerator FinishPotion()
    {
        yield return new WaitForSeconds(3f); // Espera 3 segundos

        // Cambiar al siguiente estado (ejemplo: dejar la poci�n en la mesa)
        _stateMachine.SwitchState(_controller.waitingState);
    }
}
