using System;
using System.Collections.Generic;
using UnityEngine;

public class Cat : ANPC<Cat>
{
    #region PUBLIC PROPERTIES
    [Header("Cat Properties")]
    public List<Transform> wayPoints = new();
    #endregion

    #region PRIVATE PROPERTIES
    BehaviourTree<Cat> _catBT;
    #endregion

    #region NODES
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Cat> CreateDecisionSystem()
    {
        _catBT = new(this, "Cat Behaviour Tree");
        _catBT.AddChild(new LeafNode<Cat>(this, "Walking around", new PatrolStrategy<Cat>(transform, _agent, wayPoints)));
        return _catBT;
    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {

    }
    #endregion

    #region PUBLIC	METHODS

    #endregion

    #region PRIVATE	METHODS

    #endregion
}
