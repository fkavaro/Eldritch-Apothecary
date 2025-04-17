using UnityEngine;

/// <summary>
/// Waits for its service turn sat down.
/// </summary>
public class WaitForService_ClientState : AState<Client, StackFiniteStateMachine<Client>>
{
    public WaitForService_ClientState(StackFiniteStateMachine<Client> sfsm)
    : base("Waiting for service", sfsm) { }

    public override void StartState()
    {
        _behaviourController.SetTargetSpot(ApothecaryManager.Instance.RandomWaitingSeat());
    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Has reached the waiting seat
        if (_behaviourController.HasArrived())
        {
            switch (_behaviourController.wantedService)
            {
                case Client.WantedService.Sorcerer:
                    if (ApothecaryManager.Instance.IsTurn(_behaviourController))
                        _stateMachine.SwitchState(_behaviourController.atSorcererState);
                    else
                        _behaviourController.ChangeAnimationTo(_behaviourController.sitDownAnim);
                    break;
                case Client.WantedService.Alchemist:
                    _behaviourController.ChangeAnimationTo(_behaviourController.sitDownAnim);
                    _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.pickPotionUpState, "Sitting down"));

                    // if (ApothecaryManager.Instance.IsTurn(_behaviourController))
                    //     _stateMachine.SwitchState(_behaviourController.pickPotionUpState);
                    // else
                    //     _behaviourController.ChangeAnimationTo(_behaviourController.sitDownAnim);
                    break;
                default:
                    _behaviourController.ChangeAnimationTo(_behaviourController.sitDownAnim);
                    _behaviourController.StartCoroutine(WaitAndSwitchState(_behaviourController.leavingState, "Sitting down"));
                    break;
            }

        }
    }
}
