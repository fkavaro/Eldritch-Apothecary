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

    #region VARIABLES
    [Header("Agent Properties")]
    [Tooltip("Agent speed"), Range(0f, 5f)]
    public float agentSpeed = 3.5f;
    [Tooltip("Threshold for target position sampling"), Range(0f, 1f)]
    public float targetThreshold = 1f;
    [Tooltip("Minimum distance to the target to consider the agent has arrived"), Range(0f, 1f)]
    public float minDistanceToTarget = 0.3f;
    [Tooltip("Whether to draw debug gizmos in the scene view")]
    [SerializeField] bool drawDebugGizmos;
    [Tooltip("Color of the debug target gizmo")]
    [SerializeField] Color targetDebugColor = Color.green;
    #endregion

    /// <summary>
    /// Sets the NavMeshAgent component and initializes its speed.
    /// </summary>
    protected override void OnAwake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = agentSpeed;

        base.OnAwake(); // Sets the animator component
    }

    /// <summary>
    /// Sets the target position for the NavMeshAgent to navigate to.
    /// </summary>
    /// <param name="targetPos">The target position in world coordinates.</param>
    public void SetTarget(Vector3 targetPos)
    {
        _agent.isStopped = false;
        _agent.SetDestination(targetPos);
        ChangeAnimationTo(Moving);
    }

    /// <summary>
    /// Checks if the NavMeshAgent has arrived at its destination.
    /// </summary>
    /// <returns>True if the agent has arrived, otherwise false.</returns>
    public bool HasArrived()
    {
        if (Vector3.Distance(transform.position, _agent.destination) < minDistanceToTarget)
        {
            //ChangeAnimationTo(Idle);
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Checks if the NavMeshAgent has arrived at certain destination.
    /// </summary>
    /// <returns>True if the agent has arrived, otherwise false.</returns>
    public bool HasArrived(Vector3 destination)
    {
        if (Vector3.Distance(transform.position, destination) < minDistanceToTarget)
        {
            //ChangeAnimationTo(Idle);
            return true;
        }
        else
            return false;
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
    /// Stops the agent from moving and resets its destination.
    /// </summary>
    public void StopAgent()
    {
        _agent.isStopped = true;
        //_agent.ResetPath();
        //ChangeAnimationTo(Idle);
    }

    /// <summary>
    /// Reactivates the agent movement.
    /// </summary>
    public void ReactivateAgent()
    {
        _agent.isStopped = false;
    }

    // /// <summary>
    // /// Draws debug gizmos in the scene view to visualize the target position.
    // /// </summary>
    // void OnDrawGizmos()
    // {
    //     if (_agent != null && drawDebugGizmos)
    //     {
    //         Gizmos.color = targetDebugColor;
    //         var target = GetTarget();
    //         Gizmos.DrawLine(transform.position, target);
    //         Gizmos.DrawSphere(target, 0.5f);
    //     }
    // }
}

