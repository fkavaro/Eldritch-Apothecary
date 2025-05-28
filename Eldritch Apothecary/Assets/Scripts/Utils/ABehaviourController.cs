using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Defines context methods. Implements MonoBehaviour.
/// </summary>
public abstract class ABehaviourController<TController> : MonoBehaviour
where TController : ABehaviourController<TController>
{
    public event Action OnCoroutineFinished;

    [Header("Behaviour Controller Properties")]
    [Tooltip("Whether to show debug messages in the console or not")]
    public bool debugMode = false;
    [Tooltip("Whether to update next frame or not")]
    [SerializeField] protected bool isExecutionPaused = false;

    [HideInInspector] public TextMeshProUGUI actionText, animationText;
    protected Transform debugCanvas;
    ADecisionSystem<TController> _decisionSystem;

    /// <summary>
    /// Create the main decision system.
    /// </summary>
    protected abstract ADecisionSystem<TController> CreateDecisionSystem();

    /// <summary>
    /// Resets the decision system.
    /// </summary>
    public void ResetBehaviour()
    {
        _decisionSystem?.Reset();
    }

    protected void InvokeOnCoroutineFinished()
    {
        OnCoroutineFinished?.Invoke();
    }

    #region UNITY EXECUTION EVENTS
    private void Awake()
    {
        debugCanvas = transform.Find("DebugCanvas")?.transform;
        actionText = debugCanvas?.Find("Action text").GetComponent<TextMeshProUGUI>();
        animationText = debugCanvas?.Find("Animation text").GetComponent<TextMeshProUGUI>();

        OnAwake();
        _decisionSystem?.Awake();
    }
    protected virtual void OnAwake() { } // Optionally implemented in subclasses

    private void Start()
    {
        OnStart();
        _decisionSystem = CreateDecisionSystem();
        _decisionSystem?.Start();
    }
    protected virtual void OnStart() { } // Optionally implemented in subclasses

    private void Update()
    {
        OnUpdate();

        // Don't update if execution is paused
        if (!isExecutionPaused)
            _decisionSystem?.Update();
    }
    protected virtual void OnUpdate() { } // Optionally implemented in subclasses
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
