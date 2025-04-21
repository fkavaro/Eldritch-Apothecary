using System;
using System.Collections.Generic;
using UnityEngine;

public class Cat : ANPC<Cat>
{
    #region PUBLIC PROPERTIES
    [Header("Cat Properties")]
    public List<Transform> wayPoints = new();
    public Transform centerPoint;
    public int targetSamplingIterations = 30;
    public float areaRadious = 10f;
    #endregion

    #region PRIVATE PROPERTIES
    BehaviourTree<Cat> _catBT;
    #endregion

    #region NODES
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Cat> CreateDecisionSystem()
    {
        // Strategies
        RandomMovementStrategy<Cat> randomMovement = new RandomMovementStrategy<Cat>(this, centerPoint, targetSamplingIterations, areaRadious);

        // Behaviour tree
        _catBT = new(this, "Cat Behaviour Tree");

        //_catBT.AddChild(new LeafNode<Cat>(this, "Walking around", new PatrolStrategy<Cat>(this, wayPoints)));
        _catBT.AddChild(new LeafNode<Cat>(this, "Walking around", randomMovement));

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
