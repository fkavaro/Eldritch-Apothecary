using UnityEngine;

/// <summary>
/// Picks up its potion
/// </summary>
public class PickPotionUp_ClientState : AClientState
{
    public PickPotionUp_ClientState(StackFiniteStateMachine stackFsm, Client client) : base(stackFsm, client) { }

    public override void StartState()
    {
        _clientContext.SetTarget(ApothecaryManager.Instance.RandomPickUp());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached pick up position
        if (_clientContext.HasArrived())
        {
            _clientContext.ChangeAnimationTo(_clientContext.pickUpAnim);
            _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.leavingState, "Picking up the potion")); // TODO: Just wait until animation is executed
        }
    }
}