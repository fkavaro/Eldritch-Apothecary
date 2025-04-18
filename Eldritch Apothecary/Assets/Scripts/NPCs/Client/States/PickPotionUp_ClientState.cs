using UnityEngine;

/// <summary>
/// Picks up its potion
/// </summary>
public class PickPotionUp_ClientState : ANPCState<Client, StackFiniteStateMachine<Client>>
{
    public PickPotionUp_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Picking potion", sfsm) { }

    public override void StartState()
    {
        _behaviourController.SetDestination(ApothecaryManager.Instance.RandomPickUp());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached pick up position
        if (_behaviourController.HasArrivedAtDestination())
            _behaviourController.StartCoroutine(WaitAndSwitchState(3f, _behaviourController.leavingState, _behaviourController.pickUpAnim, "Picking up the potion"));
    }
}