using System;
using BehaviourAPI.UtilitySystems;
using UnityEngine;

public abstract class AAction<TController> where TController : ABehaviourController<TController>
{
    public string name;

    protected TController _behaviourController;

    public AAction(string name, UtilitySystem<TController> utilitySystem, bool isDefault = false)
    {
        this.name = name;
        _behaviourController = utilitySystem.controller;
        utilitySystem.AddAction(this, isDefault);
    }

    public abstract float CalculateUtility();
    public abstract void StartAction();
    public abstract void UpdateAction();
    public abstract bool IsFinished();
}
