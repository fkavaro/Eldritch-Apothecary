using UnityEngine;

/// <summary>
/// Attends the sorcerer until its service is finished
/// </summary>
public class AtSorcerer_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{
    public AtSorcerer_ClientState(StackFiniteStateMachine<Client> sfsm) : base(sfsm)

    {
        stateName = "At Sorcerer";
    }

    public override void StartState()
    {
        _behaviourController.SetTargetSpot(ApothecaryManager.Instance.sorcererSeat);
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached the sorcerer seat
        if (_behaviourController.HasArrived())
        {
            _behaviourController.ChangeAnimationTo(_behaviourController.sitDownAnim); // TODO: stand up animation
            _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, "Sitting down"));
        }
    }
}