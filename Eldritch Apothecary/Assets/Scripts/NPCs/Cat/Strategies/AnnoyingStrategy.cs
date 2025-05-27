using UnityEngine;

public class AnnoyingStrategy<TController> : AStrategy<TController>
where TController : ANPC<TController>
{
    readonly Transform positionWhereAnnoy;

    public AnnoyingStrategy(TController controller, Transform positionWhereAnnoy) : base(controller)
    {
        this.positionWhereAnnoy = positionWhereAnnoy;
    }

    public override Node<TController>.Status Update()
    {
        // Set position where annoy as destination if it's not already
        if (_controller.GetDestinationPos() != positionWhereAnnoy.position)
            _controller.SetDestination(positionWhereAnnoy.position);

        // Keep annoying for some time if destination arrived
        if (_controller.HasArrivedAtDestination())
        {
            _controller.PlayAnimationRandomTime(_controller.idleAnim, "Annoying");
            return Node<TController>.Status.Success;
        }

        return Node<TController>.Status.Running;
    }
}