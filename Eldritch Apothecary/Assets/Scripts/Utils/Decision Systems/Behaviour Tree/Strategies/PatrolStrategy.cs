using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// PatrolStrategy is a strategy for patrolling between a list of points using a NavMeshAgent.
/// </summary>
public class PatrolStrategy<TController> : IStrategy<TController>
where TController : ABehaviourController<TController>
{
    readonly Transform _entity;
    readonly NavMeshAgent _agent;
    readonly List<Transform> _patrolPoints;
    int _currentPatrolPointIndex;
    bool isPathCalculated;

    public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints)
    {
        _entity = entity;
        _agent = agent;
        _patrolPoints = patrolPoints;
    }

    public Node<TController>.Status Update()
    {
        if (_currentPatrolPointIndex >= _patrolPoints.Count)
            return Node<TController>.Status.Success;

        var target = _patrolPoints[_currentPatrolPointIndex];
        _agent.SetDestination(target.position);
        _entity.LookAt(target);

        if (isPathCalculated && _agent.remainingDistance <= 0.1f)
        {
            _currentPatrolPointIndex++;
            isPathCalculated = false;
        }

        if (_agent.pathPending)
        {
            isPathCalculated = true;
        }

        return Node<TController>.Status.Running;
    }

    public void Reset()
    {
        _currentPatrolPointIndex = 0;
    }
}
