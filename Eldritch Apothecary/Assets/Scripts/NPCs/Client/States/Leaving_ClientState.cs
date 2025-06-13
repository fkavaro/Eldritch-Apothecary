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
        // Is close to the apothecary exit
        if (_controller.IsCloseTo(exitPosition, 3f))
        {
            ApothecaryManager.Instance.clientsPool.Release(_controller);
            // Sets scale to 1 in case it was modified by a failed spell
            _controller.ResetScale();
            _controller.ResetColor();

            // Checks if client is still on the sorcerer clients list
            if (_controller.wantedService == Client.WantedService.SPELL &&
                ApothecaryManager.Instance.sorcererClientsQueue.Contains(_controller))
            {
                // Updates sorcerer clients list
                ApothecaryManager.Instance.sorcererClientsQueue.Remove(_controller);
            }

            // Wanted a potion
            else if (_controller.wantedService == Client.WantedService.POTION)
                ApothecaryManager.Instance.GoneClient(_controller);
        }
        //Is close to the queue exit
        else if (_controller.IsCloseTo(queueExitPosition, 3f))
            _controller.SetDestination(exitPosition);

    }
}