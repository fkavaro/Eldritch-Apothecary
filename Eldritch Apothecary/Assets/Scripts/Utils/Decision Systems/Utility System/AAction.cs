using System;
using BehaviourAPI.UtilitySystems;
using UnityEngine;

public abstract class AAction<TController, TFactor> : IAction where TController : ABehaviourController<TController>
{
    public string Name => name;
    public float Utility => CalculateUtility();

    protected string name;
    protected float utility;
    protected TController _behaviourController;
    protected TFactor _decisionFactor => SetDecisionFactor();

    public AAction(string name, UtilitySystem<TController> utilitySystem)
    {
        this.name = name;
        _behaviourController = utilitySystem.controller;
        utilitySystem.AddAction(this);
    }

    protected abstract TFactor SetDecisionFactor();
    protected abstract float CalculateUtility();
    public abstract void StartAction();
    public abstract void UpdateAction();
    public abstract bool IsFinished();
}
