using UnityEngine;

public class AttendingClients_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    int spellCastingTime = Random.Range(5, 10); // Devuelve un entero entre 0 y 9

    private float waitTimer = 0f;
    private bool isWaiting = false;
    
    public AttendingClients_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
    : base("Attending clients", sfsm) { }

    public override void StartState()
    {
        _controller.SetDestinationSpot(ApothecaryManager.Instance.sorcererSeat);
    }

    public override void UpdateState()
    {
        if (_controller.HasArrivedAtDestination())
        {
            if (!isWaiting)
            {
                _controller.ChangeAnimationTo(_controller.sitDownAnim);
                _controller.ChangeAnimationTo(_controller.castSpellAnim);
                _controller.CastSpellEffect(spellCastingTime);
                isWaiting = true;
                waitTimer = 0f;
            }

            waitTimer += Time.deltaTime;

            if (waitTimer >= spellCastingTime)
            {
                ApothecaryManager.Instance.NextSorcererTurn();
                // Cambia al estado de Pickup Potion despu�s de esperar 5 segundos
                SwitchState(_controller.waitForClientState);
                //SwitchState(_controller.pickUpIngredientsState);

                // Reinicia para evitar que vuelva a entrar en este bloque
                waitTimer = 0f;
                isWaiting = false;
            }
        }
    }
}
