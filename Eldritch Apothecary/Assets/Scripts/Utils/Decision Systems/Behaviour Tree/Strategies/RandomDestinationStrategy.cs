using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// RandomDestinationStrategy is a strategy for calculating a random destination using a NavMeshAgent.
/// </summary>
public class RandomDestinationStrategy<TController> : RandomPatrolStrategy<TController>
where TController : ANPC<TController>
{
    bool _destinationIsSet = false; // Dirty flag

    public RandomDestinationStrategy(TController controller, Transform centerPoint, int samplingIterations = 30, float areaRadious = 10f)
    : base(controller, centerPoint, samplingIterations, areaRadious) { }


    public override Node<TController>.Status Update()
    {
        // Destination not yet set
        if (!_destinationIsSet)
        {
            // Random destination is reachable
            if (_controller.CalculateRandomDestination(_samplingIterations, _areaRadious, _centerPoint, out Vector3 randomDestination))
            {
                _controller.SetDestination(randomDestination);
                _destinationIsSet = true;
            }
            // It's not
            else
                return Node<TController>.Status.Failure;
        }

        // Has arrived
        if (_controller.HasArrivedAtDestination())
            // Success
            return Node<TController>.Status.Success;
        else // Hasn't arrived
        {
            // Reduce energy
            _controller.ReduceEnergy(Time.deltaTime);

            // Running
            return Node<TController>.Status.Running;
        }
    }

    public override void Reset()
    {
        _destinationIsSet = false;
    }
}