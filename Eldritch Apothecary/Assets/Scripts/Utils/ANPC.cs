using Unity.VisualScripting;
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
    protected NavMeshAgent _agent;
    Spot _destinationSpot = null;

    /// <summary>
    /// Animation to play when the agent arrives at target
    /// </summary>
    int _animationWhenArrived = -1;

    #region VARIABLES
    [Header("NavMeshAgent Properties")]
    [Tooltip("Agent speed"), Range(0f, 5f)]
    public float speed = 2f;
    [Tooltip("Agent rotation speed"), Range(0f, 5f)]
    public float rotationSpeed = 3f;
    [Tooltip("Max distance from the random point to a point on the navmesh, for target position sampling"), Range(0f, 5f)]
    public float maxSamplingDistance = 1f;
    [Tooltip("Distance to which it's considered as arrived"), Range(0.3f, 1f)]
    public float stoppingDistance = 0.3f;
    [Tooltip("Distance to which it's close to the destination"), Range(2f, 5f)]
    public float nearDistance = 2f;
    [Tooltip("Distance to which the agent will avoid other agents"), Range(0.5f, 2f)]
    public float avoidanceRadius = 0.7f;
    public bool isStopped;

    [Header("Energy Properties")]
    [Tooltip("Energy value"), Range(0, 100)]
    public float energy = 100;
    [Tooltip("Energy is low below this value"), Range(10, 60)]
    public float lowEnergyThreshold = 10;
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
        _agent.stoppingDistance = stoppingDistance;
        _agent.radius = avoidanceRadius;

        base.OnAwake(); // Sets the animator component
    }

    protected override void OnUpdate()
    {
        // Stop moving if execution is paused
        isStopped = isExecutionPaused;
        _agent.isStopped = isStopped;
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Sets the target position for the NavMeshAgent to navigate to
    /// and optionally the animation to play when arriving.
    /// </summary>
    public void SetDestinationSpot(Spot destinationSpot, int animationWhenArrived = -1)
    {
        if (_destinationSpot == destinationSpot) return;

        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("SetDestinationSpot(): NavMeshAgent is not on a NavMesh.");
            return;
        }

        SetDestination(destinationSpot.transform.position, animationWhenArrived); // Set the target position for the NavMeshAgent

        _destinationSpot = destinationSpot;
    }

    /// <summary>
    /// Sets the target position for the NavMeshAgent to navigate to
    /// and optionally the animation to play when arriving.
    /// </summary>
    public void SetDestination(Vector3 destinationPos, int animationWhenArrived = -1)
    {
        if (_agent.destination == destinationPos) return;

        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("SetDestination(): NavMeshAgent is not on a NavMesh.");
            return;
        }

        if (_destinationSpot != null)
        {
            _destinationSpot.SetOccupied(false); // Leave free current target spot
            _destinationSpot = null; // Reset the target spot
        }

        if (animationWhenArrived != -1)
            _animationWhenArrived = animationWhenArrived; // Set the animation to play when arriving

        _agent.updateRotation = true;
        _agent.SetDestination(destinationPos);

        if (HasArrivedAtDestination()) return;
        else ChangeAnimationTo(walkAnim);
    }

    public bool DestinationSpotIsOccupied()
    {
        if (_destinationSpot == null)
            return false;
        else
            return _destinationSpot.IsOccupied();
    }

    public bool IsCloseToDestination(float checkingDistance = 2f)
    {
        return IsCloseTo(_agent.destination, checkingDistance);
    }

    public bool IsCloseTo(Vector3 destination, float checkingDistance = 2f)
    {
        if (checkingDistance <= nearDistance)
            checkingDistance = nearDistance;

        if (Vector3.Distance(transform.position, destination) < checkingDistance)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Checks if the NavMeshAgent has arrived at its destination, 
    /// and if the target is a spot, fixes its rotation if wanted.
    /// </summary>
    /// <returns>True if the agent has arrived, otherwise false.</returns>
    public bool HasArrivedAtDestination(bool fixRotation = true, bool fixPosition = true)
    {
        return HasArrived(_agent.destination, fixRotation, fixPosition);
    }

    /// <summary>
    /// Checks if the NavMeshAgent has arrived at certain destination
    /// and if the target is a spot, fixes its rotation if wanted.
    /// </summary>
    /// <returns>True if the agent has arrived, otherwise false.</returns>
    public bool HasArrived(Vector3 destination, bool fixRotation = true, bool fixPosition = true)
    {
        //Debug.Log($"{gameObject.name} is checking if it has arrived at {destination}.");

        if (Vector3.Distance(transform.position, destination) < stoppingDistance)
        //if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            //Debug.Log($"{gameObject.name} has arrived at {destination}.");

            if (_destinationSpot != null)
            {
                _destinationSpot.SetOccupied(true);

                if (fixRotation)
                    ForceRotation(_destinationSpot.DirectionToVector()); // Fix rotation to the target position
                if (fixPosition)
                    transform.position = _destinationSpot.transform.position;
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

    public void ForceRotation(Vector3 lookDirection)
    {
        if (_agent.isOnNavMesh)
            _agent.updateRotation = false; // Disable automatic rotation

        transform.rotation = Quaternion.Euler(lookDirection);
    }
    public void ForceRotation(Quaternion rotation)
    {
        if (_agent.isOnNavMesh)
            _agent.updateRotation = false; // Disable automatic rotation

        transform.rotation = rotation;
    }

    /// <summary>
    /// Checks if the NavMeshAgent can move to the specified position,
    /// taking out its nearest reachable position.
    /// </summary>
    public bool CanReachPosition(Vector3 targetPos, out Vector3 reachablePos)
    {
        NavMeshHit hitLocation;

        if (NavMesh.SamplePosition(targetPos, out hitLocation, maxSamplingDistance, NavMesh.AllAreas))
        {
            reachablePos = hitLocation.position;
            return true;
        }
        else
        {
            reachablePos = Vector3.zero;
            return false;
        }
    }

    /// <summary>
    /// Returns true if a random point is reachable
    /// </summary>
    public bool CalculateRandomDestination(int samplingIterations, float areaRadious, Transform centerPoint, out Vector3 destination)
    {
        // Repeat until a random position in the navmesh is found
        for (int i = 0; i < samplingIterations; i++)
        {
            // Random point inside a circular area
            Vector3 randomPoint = centerPoint.position + UnityEngine.Random.insideUnitSphere * areaRadious;

            // Try to find a position in the navmesh area sampled from the random position
            if (CanReachPosition(randomPoint, out destination))
                return true;
        }

        // Hasn't found any reachable point in the navmesh
        destination = Vector3.zero;
        return false;
    }

    /// <summary>
    /// Sets the NavMeshAgent to be stopped or not.
    /// </summary>
    public void SetIfStopped(bool isStopped)
    {
        if (!_agent.isOnNavMesh)
        {
            Debug.LogError("IsStopped(): NavMeshAgent is not on a NavMesh.");
            return;
        }

        _agent.isStopped = isStopped;
    }

    /// <summary>
    /// Sets the NavMeshAgent's speed.
    /// </summary>
    public void SetAvoidanceRadius(float radius)
    {
        _agent.radius = radius;
    }

    /// <summary>
    /// Resets the NavMeshAgent's avoidance radius to its default value.
    /// </summary>
    public void ResetAvoidanceRadius()
    {
        _agent.radius = avoidanceRadius;
    }

    public bool IsPathPending()
    {
        return _agent.pathPending;
    }

    /// <summary>
    /// Gets the current target position of the NavMeshAgent.
    /// </summary>
    /// <returns>The target position in world coordinates.</returns>
    public Vector3 GetDestinationPos()
    {
        return _agent.destination;
    }

    public void ReduceEnergy(float amount)
    {
        if (energy > 0)
            energy -= amount;
    }

    public void IncreaseEnergy(float amount)
    {
        if (energy < 100)
            energy += amount;
    }

    public bool IsEnergyLow()
    {
        return energy <= lowEnergyThreshold;
    }

    public bool IsEnergyAtMax()
    {
        return energy >= 100;
    }
    #endregion
}

