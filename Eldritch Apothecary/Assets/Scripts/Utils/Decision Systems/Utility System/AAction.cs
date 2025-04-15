using BehaviourAPI.UtilitySystems;
using UnityEngine;

public abstract class AAction<TController> where TController : ABehaviourController<TController>
{
    public string name;

    protected TController _behaviourController;

    public AAction(string name, UtilitySystem<TController> utilitySystem)
    {
        this.name = name;
        _behaviourController = utilitySystem.controller;
        utilitySystem.AddAction(this);
    }

    public abstract float CalculateUtility(TController controller);
    public abstract void Execute(TController controller);
}
