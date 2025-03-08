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
        // If client has reached the entrance after complained
        if (clientContext.HasArrived(ApothecaryManager.Instance.entrancePosition.position))
        {
            // Destroy gameobject
            GameObject.Destroy(clientContext.gameObject); // TODO: Return to clients pool
        }
        // If client has reached the complaining position
        else if (clientContext.HasArrived())
        {
            // Complaining animation
            clientContext.ChangeAnimationTo(clientContext.Talking); // TODO: Change to complaining animation

            // Leave apothecary
            clientContext.SetTarget(ApothecaryManager.Instance.entrancePosition.position);
        }
    }

    public override void ExitState()
    {

    }
}
