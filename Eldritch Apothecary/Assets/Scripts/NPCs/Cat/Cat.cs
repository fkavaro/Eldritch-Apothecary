using System;
using System.Collections.Generic;
using UnityEngine;

public class Cat : ANPC<Cat>
{
    #region PUBLIC PROPERTIES
    [Header("Cat Properties")]
    public List<Transform> wayPoints = new();
    public Transform centerPoint;
    [Tooltip("Maximum iterations allowed to calculate a destination"), Range(5, 30)]
    public int targetSamplingIterations = 30;
    [Tooltip("Movement radious"), Range(5f, 20f)]
    public float areaRadious = 10f;
    [Tooltip("Minimum distance to detect someone to bother"), Range(2f, 5f)]
    public float botherDistance = 5f;
    public Transform alchemistTable,
        sorcererTable;
    #endregion

    #region PRIVATE PROPERTIES
    #endregion

    #region NODES
    BehaviourTree<Cat> _catBT;
    InfiniteLoopNode<Cat> infiniteLoop;
    SequenceNode<Cat> behaviourSequence,
        annoyAlchemistSequence,
        annoySorcererSequence,
        restSequence;
    SelectorNode<Cat> annoyWorkerSelector;
    LeafNode<Cat> isAlchemistNearLeaf,
        annoyAlchemistLeaf,
        isSorcererNearLeaf,
        annoySorcererLeaf,
        walkAroundLeaf,
        isEnergyLowConditionLeaf,
        restLeaf;
    SuccesserNode<Cat> successerAnnoyWorker;
    #endregion

    #region STRATEGIES
    RandomDestinationStrategy<Cat> randomDestinationStrategy;
    ConditionStrategy<Cat> isAlchemistNearStrategy,
        isSorcererNearStrategy,
        isEnergyLowStrategy;
    RestStrategy<Cat> restStrategy;
    AnnoyingStrategy<Cat> annoyingAlchemistStrategy,
        annoyingSorcererStrategy;
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Cat> CreateDecisionSystem()
    {
        // Strategies
        randomDestinationStrategy = new(this, centerPoint, targetSamplingIterations, areaRadious);
        isAlchemistNearStrategy = new(this, IsAlchemistNear);
        annoyingAlchemistStrategy = new(this, alchemistTable);
        isSorcererNearStrategy = new(this, IsSorcererNear);
        annoyingSorcererStrategy = new(this, sorcererTable);
        isEnergyLowStrategy = new(this, IsEnergyLow);
        restStrategy = new(this);

        // Annoy alchemist sequence
        isAlchemistNearLeaf = new(this, "IsAlchemistNear", isAlchemistNearStrategy);
        annoyAlchemistLeaf = new(this, "Annoying Alchemist", annoyingAlchemistStrategy);
        annoyAlchemistSequence = new(this);
        annoyAlchemistSequence.AddChild(isAlchemistNearLeaf);
        annoyAlchemistSequence.AddChild(annoyAlchemistLeaf);

        // Annoy sorcerer sequence
        isSorcererNearLeaf = new(this, "IsSorcererNear", isSorcererNearStrategy);
        annoySorcererLeaf = new(this, "Annoying Sorcerer", annoyingSorcererStrategy);
        annoySorcererSequence = new(this);
        annoyAlchemistSequence.AddChild(isSorcererNearLeaf);
        annoySorcererSequence.AddChild(annoySorcererLeaf);

        // Annoy worker selector
        annoyWorkerSelector = new(this);
        annoyWorkerSelector.AddChild(annoyAlchemistSequence);
        annoyWorkerSelector.AddChild(annoySorcererSequence);

        // Selector succeder
        successerAnnoyWorker = new(this);
        successerAnnoyWorker.AddChild(annoyWorkerSelector);

        // Walk around
        walkAroundLeaf = new(this, "Walking around", randomDestinationStrategy);

        // Rest sequence
        isEnergyLowConditionLeaf = new(this, "IsEnergyLow", isEnergyLowStrategy);
        restLeaf = new(this, "Resting", restStrategy);
        restSequence = new(this);
        restSequence.AddChild(isEnergyLowConditionLeaf);
        restSequence.AddChild(restLeaf);

        // Behaviour sequence
        behaviourSequence = new(this);
        behaviourSequence.AddChild(successerAnnoyWorker);
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

    bool IsAlchemistNear()
    {
        return IsNear(alchemistTable);
    }

    bool IsSorcererNear()
    {
        return IsNear(sorcererTable);
    }

    bool IsNear(Transform whereToAnnoy)
    {
        if (whereToAnnoy == null) return false;

        return Vector3.Distance(transform.position, whereToAnnoy.position) <= botherDistance;
    }
    #endregion
}
