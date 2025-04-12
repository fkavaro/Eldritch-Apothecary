using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Defines context methods. Implements MonoBehaviour.
/// </summary>
public abstract class ABehaviourController : MonoBehaviour
{
    [Header("Behaviour Controller Properties")]
    [Tooltip("Whether to show debug messages in the console")]
    public bool debugMode = false;

    protected Transform debugCanvas;
    [HideInInspector] public TextMeshProUGUI stateText, actionText;
    ADecisionSystem _decisionSystem;

    /// <summary>
    /// Create the main decision system.
    /// </summary>
    protected abstract ADecisionSystem CreateDecisionSystem(); // Implemented in subclasses

    /// <summary>
    /// Resets the decision system.
    /// </summary>
    public void ResetBehaviour()
    {
        _decisionSystem?.Reset();
    }

    #region UNITY EXECUTION EVENTS
    private void Awake()
    {
        debugCanvas = transform.Find("DebugCanvas").transform;
        stateText = debugCanvas?.Find("StateText").GetComponent<TextMeshProUGUI>();
        actionText = debugCanvas?.Find("ActionText").GetComponent<TextMeshProUGUI>();

        OnAwake();
        //_decisionSystem?.Awake(); NO
    }
    protected abstract void OnAwake(); // Implemented in subclasses

    private void Start()
    {
        OnStart();
        _decisionSystem = CreateDecisionSystem();
        stateText.gameObject.SetActive(debugMode);
        //_decisionSystem?.Start(); NO
    }
    protected abstract void OnStart(); // Implemented in subclasses

    private void Update()
    {
        if (stateText.gameObject.activeSelf != debugMode)
        {
            stateText.gameObject.SetActive(debugMode);
            actionText.gameObject.SetActive(debugMode);
        }

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
