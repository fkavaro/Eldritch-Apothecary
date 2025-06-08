using UnityEngine;
using UnityEngine.Rendering;

public class WaitForClient_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    public WaitForClient_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
        : base("Waiting for client", sfsm) { }

    public override void StartState()
    {
        if (ApothecaryManager.Instance.currentSorcererTurn == 0)
        {
            ApothecaryManager.Instance.NextSorcererTurn();
        }
        _controller.SetDestinationSpot(ApothecaryManager.Instance.sorcererSeat);
    }

    public override void UpdateState()
    {
        if (_controller.HasArrivedAtDestination())
        {
            // Sit
            _controller.ChangeAnimationTo(_controller.sitDownAnim);
            // A client is as well
            if (ApothecaryManager.Instance.clientSeat.IsOccupied())
                // Espera aleatoria
                SwitchState(_controller.pickUpIngredientsState);
        }

        if (ApothecaryManager.Instance.sorcererClientsQueue.Count == 0)
        {
            return;
        }

        if (ApothecaryManager.Instance.sorcererClientsQueue[0].turnNumber == -1)
        {
            return;
        }

        if (ApothecaryManager.Instance.sorcererClientsQueue.Count > 0)
        {
            ApothecaryManager.Instance.currentSorcererTurn = ApothecaryManager.Instance.sorcererClientsQueue[0].turnNumber;
        }
    }
}