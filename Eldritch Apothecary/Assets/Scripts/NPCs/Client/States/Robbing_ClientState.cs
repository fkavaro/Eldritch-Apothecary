using System;
using UnityEngine;

public class Robbing_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    Vector3 entrancePosition;
    float previousSpeed;

    public Robbing_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Robbing", sfsm) { }

    public override void StartState()
    {
        entrancePosition = ApothecaryManager.Instance.entrancePosition.position;

        _controller.speed = 5f;
        _controller.turnText.text = "!";
        _controller.SetDestination(entrancePosition);
    }

    public override void UpdateState()
    {
        if (ApothecaryManager.Instance.isSomeoneRobbing == false)
            ApothecaryManager.Instance.isSomeoneRobbing = true;

        // Is close to entrance
        if (_controller.IsCloseTo(entrancePosition))
            ApothecaryManager.Instance.clientsPool.Release(_controller);
    }

    public override void ExitState()
    {
        ApothecaryManager.Instance.isSomeoneRobbing = false;
    }
}
