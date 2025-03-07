using System;
using UnityEngine;

/// <summary>
/// Defines context methods. Implements MonoBehaviour.
/// Controls states' actions (switching states, detect collisions...)
/// </summary>
public abstract class ABehaviourController : MonoBehaviour
{
    /// <summary>
    /// Debug mode
    /// </summary>
    public bool debugMode = true;

    ADecisionSystem _decisionSystem;

    /// <summary>
    /// Create the main decision system.
    /// </summary>
    protected abstract ADecisionSystem CreateDecisionSystem(); // Implemented in subclasses

    #region UNITY EXECUTION EVENTS
    private void Awake()
    {
        OnAwake();
        _decisionSystem = CreateDecisionSystem();
        _decisionSystem?.Awake();
    }
    protected abstract void OnAwake(); // Implemented in subclasses

    private void Start()
    {
        OnStart();
        _decisionSystem?.Start();
    }
    protected abstract void OnStart(); // Implemented in subclasses

    private void Update()
    {
        OnUpdate();
        _decisionSystem?.Update();
    }
    protected abstract void OnUpdate(); // Implemented in subclasses
    # endregion

    # region COLLISION AND TRIGGER EVENTS
    private void OnCollisionEnter(Collision collision)
    {
        _decisionSystem?.OnCollisionEnter(collision);
        AtOnCollisionEnter(collision);
    }
    protected virtual void AtOnCollisionEnter(Collision collision) { } // Optionally implemented in subclasses

    private void OnCollisionStay(Collision collision)
    {
        _decisionSystem?.OnCollisionStay(collision);
        AtOnCollisionStay(collision);
    }
    protected virtual void AtOnCollisionStay(Collision collision) { } // Optionally implemented in subclasses

    private void OnCollisionExit(Collision collision)
    {
        _decisionSystem?.OnCollisionExit(collision);
        AtOnCollisionExit(collision);
    }
    protected virtual void AtOnCollisionExit(Collision collision) { } // Optionally implemented in subclasses


    private void OnTriggerEnter(Collider other)
    {
        _decisionSystem?.OnTriggerEnter(other);
        AtOnTriggerEnter(other);
    }
    protected virtual void AtOnTriggerEnter(Collider other) { } // Optionally implemented in subclasses

    private void OnTriggerStay(Collider other)
    {
        _decisionSystem?.OnTriggerEnter(other);
        AtOnTriggerStay(other);
    }
    protected virtual void AtOnTriggerStay(Collider other) { } // Optionally implemented in subclasses

    private void OnTriggerExit(Collider other)
    {
        _decisionSystem?.OnTriggerEnter(other);
        AtOnTriggerExit(other);
    }
    protected virtual void AtOnTriggerExit(Collider other) { } // Optionally implemented in subclasses
    #endregion
}
