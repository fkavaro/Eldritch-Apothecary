using System;
using System.Collections.Generic;
using UnityEngine;

public class Cat : ANPC<Cat>
{
    public enum Personality
    {
        NORMAL, // Less annoy distance and more min seconds between annoying the same worker
        TROUBLESOME, // More annoy distance and less min seconds between annoying the same worker
    }

    #region PUBLIC PROPERTIES
    [Header("Personality Properties")]
    public Personality personality = Personality.NORMAL;
    [Tooltip("Minimum distance to detect someone to annoy"), Range(2, 6)]
    public int annoyDistance = 4;
    [Tooltip("Minimum seconds between annoying the same worker")]
    public float minSecondsBeforeAnnoying = 120f;

    [Header("Random Movement Properties")]
    [Tooltip("Maximum iterations allowed to calculate a destination"), Range(5, 30)]
    public int targetSamplingIterations = 30;
    [Tooltip("Movement radious"), Range(1, 20)]
    public int movementRadious = 10;

    [Header("Where to annoy")]
    public Table alchemistTable;
    public Table sorcererTable;
    [HideInInspector] public float lastTimeAlchemistWasAnnoyed = -Mathf.Infinity;
    [HideInInspector] public float lastTimeSorcererWasAnnoyed = -Mathf.Infinity;
    public static event Action OnSorcererAnnoyed;
    public static event Action OnSorcererNoLongerAnnoyed;

    public static event Action OnPuddle;


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
        restLeaf,
        canAnnoyAlchemistLeaf,
        canAnnoySorcererLeaf;
    SuccederNode<Cat> successerAnnoyWorker;
    #endregion

    #region STRATEGIES
    RandomDestinationStrategy<Cat> randomDestinationStrategy;
    ConditionStrategy<Cat> isAlchemistNearStrategy,
        isSorcererNearStrategy,
        isEnergyLowStrategy,
        canAnnoyAlchemistStrategy,
        canAnnoySorcererStrategy;
    Resting_CatStrategy<Cat> restStrategy;
    Annoying_CatStrategy annoyingAlchemistStrategy;
    Annoying_CatStrategy annoyingSorcererStrategy;
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Cat> CreateDecisionSystem()
    {
        // Strategies
        randomDestinationStrategy = new(this, targetSamplingIterations, movementRadious);
        isAlchemistNearStrategy = new(this, IsAlchemistNear);
        annoyingAlchemistStrategy = new(this, alchemistTable.annoyPosition, true);
        isSorcererNearStrategy = new(this, IsSorcererNear);
        annoyingSorcererStrategy = new(this, sorcererTable.annoyPosition, false);
        isEnergyLowStrategy = new(this, IsEnergyLow);
        restStrategy = new(this);
        canAnnoyAlchemistStrategy = new(this, CanAnnoyAlchemist);
        canAnnoySorcererStrategy = new(this, CanAnnoySorcerer);

        // Annoy alchemist sequence
        canAnnoyAlchemistLeaf = new(this, "CanAnnoyAlchemist", canAnnoyAlchemistStrategy);
        isAlchemistNearLeaf = new(this, "IsAlchemistNear", isAlchemistNearStrategy);
        annoyAlchemistLeaf = new(this, "Annoying Alchemist", annoyingAlchemistStrategy);
        annoyAlchemistSequence = new(this);
        annoyAlchemistSequence.AddChild(isAlchemistNearLeaf);
        annoyAlchemistSequence.AddChild(canAnnoyAlchemistLeaf);
        annoyAlchemistSequence.AddChild(annoyAlchemistLeaf);

        // Annoy sorcerer sequence
        canAnnoySorcererLeaf = new(this, "CanAnnoySorcerer", canAnnoySorcererStrategy);
        isSorcererNearLeaf = new(this, "IsSorcererNear", isSorcererNearStrategy);
        annoySorcererLeaf = new(this, "Annoying Sorcerer", annoyingSorcererStrategy);
        annoySorcererSequence = new(this);
        annoySorcererSequence.AddChild(isSorcererNearLeaf);
        annoySorcererSequence.AddChild(canAnnoySorcererLeaf);
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

    protected override void OnAwake()
    {
        base.OnAwake();

        alchemistTable = GameObject.FindGameObjectWithTag("Alchemist table").GetComponent<Table>();
        sorcererTable = GameObject.FindGameObjectWithTag("Sorcerer table").GetComponent<Table>();


        personality = (Personality)UnityEngine.Random.Range(0, 2); // Chooses a personality randomly

        switch (personality)
        {
            case Personality.NORMAL:
                speed = UnityEngine.Random.Range(2, 4);
                annoyDistance = UnityEngine.Random.Range(2, 4);
                minSecondsBeforeAnnoying = UnityEngine.Random.Range(120f, 240f);
                break;
            case Personality.TROUBLESOME:
                speed = UnityEngine.Random.Range(3, 5);
                annoyDistance = UnityEngine.Random.Range(4, 7);
                minSecondsBeforeAnnoying = UnityEngine.Random.Range(60f, 120f);
                break;
        }
    }
    #endregion

    public Spot CloserRestingSpot()
    {
        float minDistance = float.PositiveInfinity;
        Spot closestRestingSpot = null;

        foreach (Spot spot in ApothecaryManager.Instance.catRestingSpots)
        {
            float currentDistance = Vector3.Distance(spot.transform.position, this.transform.position);

            if (currentDistance <= minDistance)
            {
                minDistance = currentDistance;
                closestRestingSpot = spot;
            }
        }

        return closestRestingSpot;
    }

    #region PRIVATE	METHODS
    bool CanAnnoyAlchemist()
    {
        if ((Time.time - lastTimeAlchemistWasAnnoyed) >= minSecondsBeforeAnnoying)
        {
            if (debugMode) Debug.Log("Cat can annoy alchemist");
            return true;
        }
        else
            return false;
    }

    bool CanAnnoySorcerer()
    {
        if ((Time.time - lastTimeSorcererWasAnnoyed) >= minSecondsBeforeAnnoying)
        {
            if (debugMode) Debug.Log("Cat can annoy sorcerer");
            OnSorcererAnnoyed?.Invoke();
            return true;
        }
        else
            return false;
    }

    bool IsAlchemistNear()
    {
        if (IsNear(alchemistTable.annoyPosition))
        {
            if (debugMode) Debug.Log("Cat is near alchemist");
            return true;
        }
        else
            return false;
    }

    bool IsSorcererNear()
    {
        if (IsNear(sorcererTable.annoyPosition))
        {
            if (debugMode) Debug.Log("Cat is near sorcerer");
            return true;
        }
        else
            return false;
    }

    bool IsNear(Transform whereToAnnoy)
    {
        if (whereToAnnoy == null) return false;

        float distance = Vector3.Distance(transform.position, whereToAnnoy.position);
        bool isNear = distance <= annoyDistance;

        return isNear;
    }


    #endregion

    public static void RaiseSorcererNoLongerAnnoyed()
    {
        OnSorcererNoLongerAnnoyed?.Invoke();
    }

}
