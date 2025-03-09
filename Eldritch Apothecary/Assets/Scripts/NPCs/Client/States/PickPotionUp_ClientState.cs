using UnityEngine;

/// <summary>
/// Picks up its potion
/// </summary>
public class PickPotionUp_ClientState : AClientState
{
    public PickPotionUp_ClientState(StackFiniteStateMachine stackFsm, Client client) : base(stackFsm, client) { }

    public override void StartState()
    {
        clientContext.SetTarget(ApothecaryManager.Instance.RandomPickUp());
    }

    public override void UpdateState()
    {
        if (coroutineStarted) return;

        // Has reached pick up position
        if (clientContext.HasArrived())
        {
            clientContext.ChangeAnimationTo(clientContext.pickUpAnim);
            clientContext.StartCoroutine(WaitAndSwitchState(clientContext.leavingState, "Picking up the potion")); // TODO: Just wait until animation is executed
        }
    }
}