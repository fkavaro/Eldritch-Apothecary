using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// PatrolStrategy is a strategy for patrolling between a list of points using a NavMeshAgent.
/// </summary>
public class PatrolStrategy : IStrategy
{
    readonly Transform _entity;
    readonly NavMeshAgent _agent;
    readonly List<Transform> _patrolPoints;
    int _currentPatrolPointIndex;
    bool isPathCalculated;

    public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed)
    {
        _entity = entity;
        _agent = agent;
        _patrolPoints = patrolPoints;
    }

    public Node.Status Update()
    {
        if (_currentPatrolPointIndex >= _patrolPoints.Count)
            return Node.Status.Success;

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

        return Node.Status.Running;
    }

    public void Reset()
    {
        _currentPatrolPointIndex = 0;
    }
}
