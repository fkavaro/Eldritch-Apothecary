using UnityEngine;

public abstract class ADecisionSystem<TController> where TController : ABehaviourController<TController>
{
    public TController controller;

    public ADecisionSystem(TController controller)
    {
        this.controller = controller;
    }

    protected abstract void Debug();

    public abstract void Awake(); // Implemented in subclasses
    public abstract void Start();
    public abstract void Update();

    public abstract void Reset();

    public virtual void OnCollisionEnter(Collision collision) { }
    public virtual void OnCollisionStay(Collision collision) { }
    public virtual void OnCollisionExit(Collision collision) { }

    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerStay(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }


}