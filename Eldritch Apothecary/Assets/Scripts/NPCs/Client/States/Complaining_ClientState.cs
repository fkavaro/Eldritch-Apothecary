using UnityEngine;

public class Complaining_ClientState : AClientState
{
    public Complaining_ClientState(StackFiniteStateMachine stackFsm, Client clientContext) : base(stackFsm, clientContext) { }

    public override void StartState()
    {
        clientContext.SetTarget(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateState()
    {
        // Walk animation
        // If client has reached the exit after complained
        if (clientContext.HasArrived(ApothecaryManager.Instance.exitPosition.position))
        {
            // Destroy gameobject
            GameObject.Destroy(clientContext.gameObject); // TODO: Return to clients pool
        }
        // If client has reached the complaining position
        else if (clientContext.HasArrived())
        {
            clientContext.ChangeAnimationTo(clientContext.Complain);

            // Leave apothecary
            clientContext.SetTarget(ApothecaryManager.Instance.exitPosition.position);
        }
    }

    public override void ExitState()
    {

    }
}
