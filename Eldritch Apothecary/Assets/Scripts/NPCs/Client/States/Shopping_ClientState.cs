using BehaviourAPI.StateMachines.StackFSMs;
using UnityEngine;

/// <summary>
/// Picks up a product or waits in line directly
/// </summary>
public class Shopping_ClientState : AClientState
{
    Position _stand;

    public Shopping_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext)
    {
        stateName = "Shopping";
    }

    public override void StartState()
    {
        // Try to get a random shop stand
        _stand = ApothecaryManager.Instance.shop.RandomStand(_clientContext);

        // Leave if no available stands
        if (_stand == null)
            _stackFsm.SwitchState(_clientContext.leavingState);
        // Go to available stand
        else
        {
            _stand.SetOccupied(true);
            _clientContext.SetTarget(_stand.transform.position);
        }

    }

    public override void UpdateState()
    {
        if (_coroutineStarted) return;

        // Arrived at shop stand
        if (_clientContext.HasArrived())
        {
            // Rotate smoothly to direction
            //_clientContext.transform.rotation = Quaternion.Slerp(_clientContext.transform.rotation, Quaternion.LookRotation(_stand.ToVector()), Time.deltaTime * 5f);
            _clientContext.transform.rotation = Quaternion.LookRotation(_stand.ToVector());
            _clientContext.ChangeAnimationTo(_clientContext.pickUpAnim);
            _clientContext.StartCoroutine(WaitAndSwitchState(_clientContext.waitForReceptionistState, "Picking up objects"));
            _stand.SetOccupied(false);
        }
    }

    public override void ExitState()
    {

    }
}
