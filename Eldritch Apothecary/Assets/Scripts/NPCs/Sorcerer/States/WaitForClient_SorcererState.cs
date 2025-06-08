using UnityEngine;
using UnityEngine.Rendering;

public class WaitForClient_SorcererState : ANPCState<Sorcerer, StackFiniteStateMachine<Sorcerer>>
{
    public WaitForClient_SorcererState(StackFiniteStateMachine<Sorcerer> sfsm)
        : base("Waiting for client", sfsm) { }

    public override void StartState()
    {
        // Instances first turn
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
            // Sits
            _controller.ChangeAnimationTo(_controller.sitDownAnim);
            // If a client is also seated
            if (ApothecaryManager.Instance.clientSeat.IsOccupied())
                // Changes state to pick up ingredients
                SwitchState(_controller.pickUpIngredientsState);
        }

        // If the list of clients is empty, continues
        if (ApothecaryManager.Instance.sorcererClientsQueue.Count == 0)
        {
            return;
        }

        // If the first client in the list doesn't have an assigned turn, continues
        if (ApothecaryManager.Instance.sorcererClientsQueue[0].turnNumber == -1)
        {
            return;
        }

        // If the first client's turn is not the same as the current sorcerer turn
        if (ApothecaryManager.Instance.sorcererClientsQueue.Count > 0)
        {
            // Changes the sorcerer turn to the first client's turn (avoids the client flow from getting stuck)
            ApothecaryManager.Instance.currentSorcererTurn = ApothecaryManager.Instance.sorcererClientsQueue[0].turnNumber;
        }
    }
}