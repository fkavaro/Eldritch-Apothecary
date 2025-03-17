using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Abstract base class for NPC (Non-Player Character).
/// Requires a NavMeshAgent component to be attached to the GameObject.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class ANPC : AAnimationController
{
    NavMeshAgent _agent;
    const float MIN_DISTANCE_TO_TARGET = 0.3f;

    #region VARIABLES
    [Header("Agent Properties")]
    [Tooltip("Agent speed"), Range(0f, 5f)]
    public float speed = 3f;
    [Tooltip("Agent rotation speed"), Range(0f, 5f)]
    public float rotationSpeed = 3f;
    [Tooltip("Threshold for target position sampling"), Range(0f, 1f)]
    public float targetThreshold = 1f;
    #endregion

    /// <summary>
    /// Sets the NavMeshAgent component and initializes its speed.
    /// </summary>
    protected override void OnAwake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = speed;
        _agent.angularSpeed = rotationSpeed * 100f;

        base.OnAwake(); // Sets the animator component
    }

    /// <summary>
    /// Sets the target position for the NavMeshAgent to navigate to.
    /// </summary>
    /// <param name="targetPos">The target position in world coordinates.</param>
    public void SetTarget(Vector3 targetPos)
    {
        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("SetTarget(): NavMeshAgent is not on a NavMesh.");
            return;
        }

        _agent.isStopped = false;
        _agent.SetDestination(targetPos);
        if (HasArrived()) return;
        ChangeAnimationTo(walkAnim);
    }

    /// <summary>
    /// Checks if the NavMeshAgent has arrived at its destination.
    /// </summary>
    /// <returns>True if the agent has arrived, otherwise false.</returns>
    public bool HasArrived(float minDistance = MIN_DISTANCE_TO_TARGET)
    {
        return !_agent.pathPending && _agent.remainingDistance <= minDistance &&
        (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
    }

    /// <summary>
    /// Checks if the NavMeshAgent has arrived at certain destination.
    /// </summary>
    /// <returns>True if the agent has arrived, otherwise false.</returns>
    public bool HasArrived(Vector3 destination, float minDistance = MIN_DISTANCE_TO_TARGET)
    {
        return Vector3.Distance(transform.position, destination) < minDistance;
    }

    /// <summary>
    /// Checks if the NavMeshAgent can move to the specified target position.
    /// </summary>
    /// <param name="targetPos">The target position in world coordinates.</param>
    /// <returns>True if the agent can move to the target position, otherwise false.</returns>
    public bool CanReachTarget(Vector3 targetPos)
    {
        return NavMesh.SamplePosition(targetPos, out var _, targetThreshold, NavMesh.AllAreas);
    }

    /// <summary>
    /// Gets the current target position of the NavMeshAgent.
    /// </summary>
    /// <returns>The target position in world coordinates.</returns>
    public Vector3 GetTarget()
    {
        return _agent.destination;
    }

    /// <summary>
    /// Stops the agent from moving.
    /// </summary>
    public void StopAgent()
    {
        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("StopAgent(): NavMeshAgent is not on a NavMesh.");
            return;
        }

        _agent.isStopped = true;
        //_agent.ResetPath();
        //ChangeAnimationTo(Idle);
    }

    /// <summary>
    /// Reactivates the agent movement.
    /// </summary>
    public void ReactivateAgent()
    {
        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("ReactivateAgent(): NavMeshAgent is not on a NavMesh.");
            return;
        }

        _agent.isStopped = false;
    }
}

