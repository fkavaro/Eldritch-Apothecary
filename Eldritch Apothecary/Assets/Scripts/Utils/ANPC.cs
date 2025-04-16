using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Abstract base class for NPC (Non-Player Character).
/// Requires a NavMeshAgent component to be attached to the GameObject.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class ANPC<TController> : AAnimationController<TController>
where TController : ABehaviourController<TController>
{
    NavMeshAgent _agent;
    Spot _targetPosition = null;

    /// <summary>
    /// Animation to play when the agent is walking.
    /// </summary>
    int _animationWhenArrived = -1;

    #region VARIABLES
    [Header("Agent Properties")]
    [Tooltip("Agent speed"), Range(0f, 5f)]
    public float speed = 3f;
    [Tooltip("Agent rotation speed"), Range(0f, 5f)]
    public float rotationSpeed = 3f;
    [Tooltip("Threshold for target position sampling"), Range(0f, 1f)]
    public float targetThreshold = 1f;
    [Tooltip("Distance to which it's considered as arrived"), Range(0f, 1f)]
    public float minDistanceToTarget = 0.3f;
    #endregion

    #region INHERITED METHODS
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
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Sets the target position for the NavMeshAgent to navigate to
    /// and optionally the animation to play when arriving.
    /// </summary>
    public void SetTargetSpot(Spot targetPos, int animationWhenArrived = -1)
    {
        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("SetTarget(): NavMeshAgent is not on a NavMesh.");
            return;
        }

        if (_targetPosition != null)
            _targetPosition.SetOccupied(false); // Leave free current target position
        _targetPosition = targetPos; // Update the target position
        _targetPosition.SetOccupied(true);

        SetTargetPos(targetPos.transform.position, animationWhenArrived); // Set the target position for the NavMeshAgent
    }

    /// <summary>
    /// Sets the target position for the NavMeshAgent to navigate to
    /// and optionally the animation to play when arriving.
    /// </summary>
    public void SetTargetPos(Vector3 targetPos, int animationWhenArrived = -1)
    {
        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("SetTarget(): NavMeshAgent is not on a NavMesh.");
            return;
        }

        if (animationWhenArrived != -1)
            _animationWhenArrived = animationWhenArrived; // Set the animation to play when arriving

        _agent.isStopped = false;
        _agent.updateRotation = true;
        _agent.SetDestination(targetPos);

        if (HasArrived()) return;
        else ChangeAnimationTo(walkAnim);
    }

    /// <summary>
    /// Checks if the NavMeshAgent has arrived at its destination, 
    /// and if the target is a spot, fixes its rotation.
    /// </summary>
    /// <returns>True if the agent has arrived, otherwise false.</returns>
    public bool HasArrived()
    {
        // Check if the agent has arrived at its destination
        return HasArrived(_agent.destination);
    }

    /// <summary>
    /// Checks if the NavMeshAgent has arrived at certain destination.
    /// </summary>
    /// <returns>True if the agent has arrived, otherwise false.</returns>
    public bool HasArrived(Vector3 destination)
    {
        //Debug.Log($"{gameObject.name} is checking if it has arrived at {destination}.");

        if (Vector3.Distance(transform.position, destination) < minDistanceToTarget)
        {
            //Debug.Log($"{gameObject.name} has arrived at {destination}.");

            if (_targetPosition != null)
            {
                _agent.updateRotation = false; // Disable automatic rotation
                transform.rotation = Quaternion.Euler(_targetPosition.DirectionToVector());
                //!NO _targetPosition = null; // Reset the target position
            }

            if (_animationWhenArrived != -1)
            {
                ChangeAnimationTo(_animationWhenArrived); // Play the animation when arriving
                _animationWhenArrived = -1; // Reset the animation to play when arriving
            }

            return true;
        }
        else return false;
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
    #endregion
}

