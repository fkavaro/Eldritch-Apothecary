using System;
using System.Collections.Generic;
using UnityEngine;

public class Cat : ANPC<Client>
{
    #region PUBLIC PROPERTIES
    [Header("Cat Properties")]
    public List<Transform> wayPoints = new();
    #endregion

    #region PRIVATE PROPERTIES
    BehaviourTree _catBT;
    #endregion

    #region NODES
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Client> CreateDecisionSystem()
    {
        return null;
    }

    protected override void OnStart()
    {
        _catBT = new("Cat Behaviour Tree");
        _catBT.AddChild(new LeafNode("Walking around", new PatrolStrategy(transform, _agent, wayPoints, speed)));
    }

    protected override void OnUpdate()
    {
        _catBT.UpdateNode();
    }
    #endregion

    #region PUBLIC	METHODS

    #endregion

    #region PRIVATE	METHODS

    #endregion
}
