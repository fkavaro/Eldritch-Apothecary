using BehaviourAPI.StateMachines.StackFSMs;
using UnityEngine;

/// <summary>
/// Picks up a product or waits in line directly
/// </summary>
public class Shopping_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{
    public Shopping_ClientState(StackFiniteStateMachine<Client> sfsm) : base(sfsm)
    {
        stateName = "Shopping";
    }

    public override void StartState()
    {
        // Try to get a random shop stand
        Spot _stand = ApothecaryManager.Instance.shop.RandomStand(_behaviourController);

        // Leave if no available stands
        if (_stand == null)
            _stateMachine.SwitchState(_behaviourController.leavingState);
        // Go to available stand
        else
            _behaviourController.SetTarget(_stand);
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Arrived at shop stand
        if (_behaviourController.HasArrived())
        {
            _behaviourController.ChangeAnimationTo(_behaviourController.pickUpAnim);
            _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.waitForReceptionistState, "Picking up objects"));
        }
    }

    public override void ExitState()
    {

    }
}
