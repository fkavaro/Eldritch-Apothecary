using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomPatrolStrategy<TController> : AStrategy<TController>
where TController : ANPC<TController>
{
    readonly Transform _centerPoint;
    readonly int _samplingIterations;
    readonly float _areaRadious;
    Vector3 _randomDestination, _randomPoint;

    public RandomPatrolStrategy(TController controller, Transform centerPoint, int samplingIterations = 30, float areaRadious = 10f) : base(controller)
    {
        _centerPoint = centerPoint;
        _samplingIterations = samplingIterations;
        _areaRadious = areaRadious;
    }

    public override Node<TController>.Status Update()
    {
        if (_controller.HasArrivedAtDestination())
        {
            // Random destination is reachable
            if (CalculateRandomDestination(out _randomDestination))
                _controller.SetDestination(_randomDestination);
            // It's not
            else
                return Node<TController>.Status.Failure;
        }
        return Node<TController>.Status.Running;
    }

    /// <summary>
    /// Returns true if a random point is reachable
    /// </summary>
    bool CalculateRandomDestination(out Vector3 destination)
    {
        // Repeat until a random position in the navmesh is found
        for (int i = 0; i < _samplingIterations; i++)
        {
            // Random point inside a circular area
            _randomPoint = _centerPoint.position + UnityEngine.Random.insideUnitSphere * _areaRadious;

            // Try to find a position in the navmesh area sampled from the random position
            if (_controller.CanReachPosition(_randomPoint, out destination))
                return true;
        }

        // Hasn't found any reachable point in the navmesh
        destination = Vector3.zero;
        return false;
    }
}