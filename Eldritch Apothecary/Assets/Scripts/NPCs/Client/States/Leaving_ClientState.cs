using UnityEngine;

/// <summary>
/// Leaves the apothecary, returning to the pool.
/// </summary>
public class Leaving_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    Vector3 queueExitPosition, exitPosition;

    public Leaving_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Leaving", sfsm) { }

    public override void StartState()
    {
        _controller.DontMindAnything(); // So that it can't be stunned or complain again
        exitPosition = ApothecaryManager.Instance.exitPosition.position;
        queueExitPosition = ApothecaryManager.Instance.queueExitPosition.position;

        _controller.turnText.text = "";
        _controller.SetDestination(queueExitPosition);
    }

    public override void UpdateState()
    {
        // Is close to the queue exit
        if (_controller.IsCloseTo(queueExitPosition, 3f))
            _controller.SetDestination(exitPosition);
        // Is close to the apothecary exit
        else if (_controller.IsCloseTo(exitPosition, 3f))
        {
            ApothecaryManager.Instance.clientsPool.Release(_controller);

            // Wanted a potion
            if (_controller.wantedService == Client.WantedService.POTION)
                // Check if client's potion is left
                ApothecaryManager.Instance.IsPotionLeft(_controller);
            ApothecaryManager.Instance._turnleftPotions.Add(_controller.turnNumber);
        }
    }
}