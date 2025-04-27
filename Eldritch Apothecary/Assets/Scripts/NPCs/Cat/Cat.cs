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

    #region NODES
    BehaviourTree<Cat> _catBT;
    InfiniteLoopNode<Cat> infiniteLoop;
    SequenceNode<Cat> sequence1;
    LeafNode<Cat> walkAround;
    #endregion

    #region STRATEGIES
    RandomDestinationStrategy<Cat> randomDestination;
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Cat> CreateDecisionSystem()
    {
        // Strategies
        randomDestination = new(this, centerPoint, targetSamplingIterations, areaRadious);

        // Nodes
        walkAround = new(this, "Walking around", randomDestination);
        sequence1 = new(this);
        sequence1.AddChild(walkAround);

        infiniteLoop = new(this, sequence1);
        _catBT = new(this, infiniteLoop);

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
