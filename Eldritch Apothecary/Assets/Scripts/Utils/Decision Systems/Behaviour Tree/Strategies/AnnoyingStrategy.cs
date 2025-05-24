using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class AnnoyingStrategy<TController> : AStrategy<TController>
where TController : ANPC<TController>
{
    readonly Transform positionWhereAnnoy;
    bool finishedAnnoying = false;

    public AnnoyingStrategy(TController controller, Transform positionWhereAnnoy) : base(controller)
    {
        this.positionWhereAnnoy = positionWhereAnnoy;
        _controller.CoroutineFinishedEvent += () => finishedAnnoying = true;
    }

    public override Node<TController>.Status Update()
    {
        // Set position where annoy as destination if it's not already
        if (_controller.GetDestinationPos() != positionWhereAnnoy.position)
            _controller.SetDestination(positionWhereAnnoy.position);

        // Return success if has finished annoying (coroutine finished)
        if (finishedAnnoying)
        {
            finishedAnnoying = false;
            return Node<TController>.Status.Success;
        }
        // Keep annoying for some time if destination arrived
        else if (_controller.HasArrivedAtDestination() && !_controller.coroutineStarted)
            _controller.StartCoroutine(_controller.PlayAnimationRandomTime(_controller.idleAnim, "Annoying"));


        return Node<TController>.Status.Running;
    }
}