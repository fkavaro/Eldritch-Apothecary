using System;
using UnityEngine;

public abstract class ADecisionSystem
{
    public ABehaviourController controller;

    public ADecisionSystem(ABehaviourController controller)
    {
        this.controller = controller;
    }

    public abstract void Awake(); // Implemented in subclasses
    public abstract void Start();
    public abstract void Update();

    public abstract void OnCollisionEnter(Collision collision);
    public abstract void OnCollisionStay(Collision collision);
    public abstract void OnCollisionExit(Collision collision);

    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);

    public abstract void Reset();
}