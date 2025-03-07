using UnityEngine;

public class Complaining_ClientState : AClientState
{
    public Complaining_ClientState(StackFiniteStateMachine stackFsm, Client clientController) : base(stackFsm, clientController) { }

    public override void StartState()
    {
        clientController.SetTarget(ApothecaryManager.Instance.complainingPosition.position);
    }

    public override void UpdateState()
    {
        // Walk animation
        // If client has reached the entrance after complained
        if (clientController.HasArrived(ApothecaryManager.Instance.entrancePosition.position))
        {
            // Destroy gameobject
            GameObject.Destroy(clientController.gameObject); // TODO: Return to clients pool
        }
        // If client has reached the complaining position
        else if (clientController.HasArrived())
        {
            // Complaining animation

            // Leave apothecary
            clientController.SetTarget(ApothecaryManager.Instance.entrancePosition.position);
        }
    }

    public override void ExitState()
    {

    }
}
