using UnityEngine;

/// <summary>
/// Picks up its potion
/// </summary>
public class PickPotionUp_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{
    public PickPotionUp_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Picking potion", sfsm) { }

    public override void StartState()
    {
        _behaviourController.SetTargetPos(ApothecaryManager.Instance.RandomPickUp());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached pick up position
        if (_behaviourController.HasArrived())
        {
            _behaviourController.ChangeAnimationTo(_behaviourController.pickUpAnim);
            _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, "Picking up the potion")); // TODO: Just wait until animation is executed
        }
    }
}