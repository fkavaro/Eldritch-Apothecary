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
    SequenceNode<Cat> behaviourSequence, restSequence;
    LeafNode<Cat> walkAroundLeaf, isEnergyLowConditionLeaf, restLeaf;
    #endregion

    #region STRATEGIES
    RandomDestinationStrategy<Cat> randomDestinationStrategy;
    ConditionStrategy<Cat> isEnergyLowStrategy;
    RestStrategy<Cat> restStrategy;
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Cat> CreateDecisionSystem()
    {
        // Strategies
        randomDestinationStrategy = new(this, centerPoint, targetSamplingIterations, areaRadious);
        isEnergyLowStrategy = new(this, IsEnergyLow);
        restStrategy = new(this);

        // Nodes
        walkAroundLeaf = new(this, "Walking around", randomDestinationStrategy);
        isEnergyLowConditionLeaf = new(this, "IsEnergyLow", isEnergyLowStrategy);
        restLeaf = new(this, "Resting", restStrategy);

        restSequence = new(this);
        restSequence.AddChild(isEnergyLowConditionLeaf);
        restSequence.AddChild(restLeaf);

        behaviourSequence = new(this);
        behaviourSequence.AddChild(walkAroundLeaf);
        behaviourSequence.AddChild(restSequence);

        infiniteLoop = new(this, behaviourSequence);
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
