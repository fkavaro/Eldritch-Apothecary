using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // Position where annoy is not yet the destination
        if (_controller.GetDestinationPos() != positionWhereAnnoy.position)
            _controller.SetDestination(positionWhereAnnoy.position);
        // Has arrived at destination
        if (_controller.HasArrivedAtDestination() && !_controller.coroutineStarted)
        {
            // Stay there annoying for some time
            _controller.StartCoroutine(_controller.PlayAnimationRandomTime(_controller.idleAnim, "Annoying"));
            return Node<TController>.Status.Success;
        }

        return Node<TController>.Status.Running;
    }
}