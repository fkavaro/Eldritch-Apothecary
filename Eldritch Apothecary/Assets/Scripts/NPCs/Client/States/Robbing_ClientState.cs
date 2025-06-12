using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Robbing_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    Vector3 entrancePosition;

    public Robbing_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Robbing", sfsm) { }

    public override void StartState()
    {
        entrancePosition = ApothecaryManager.Instance.entrancePosition.position;

        _controller.turnText.text = "!";
        _controller.SetDestination(entrancePosition);
    }

    public override void UpdateState()
    {
        // Is close to entrance
        if (_controller.IsCloseTo(entrancePosition, 3f))
            ApothecaryManager.Instance.clientsPool.Release(_controller);
    }
}
