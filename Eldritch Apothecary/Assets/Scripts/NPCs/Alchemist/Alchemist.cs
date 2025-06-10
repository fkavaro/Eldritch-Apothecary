using System;
using TMPro;
using UnityEngine;

public class Alchemist : AHumanoid<Alchemist>
{
    public enum Personality
    {
        NORMAL, // Normal speed and time replenishing each stand. 0.2 to stop idling
        LAZY, // Lower speed and more time replenishing each stand. 0.3 to stop idling
        ENERGISED // Higher speed and less time replenishing each stand. 0.1 to stop idling
    }

    public enum Efficiency
    {
        NORMAL, // Normal speed and time replenishing each stand. 0.2 to stop idling
        EFFICIENT, // Lower speed and more time replenishing each stand. 0.3 to stop idling
        INEFFICIENT // Higher speed and less time replenishing each stand. 0.1 to stop idling
    }

    public enum Skill
    {
        NOOB, // Normal speed and time replenishing each stand. 0.2 to stop idling
        ADEPT, // Lower speed and more time replenishing each stand. 0.3 to stop idling
        MASTER // Higher speed and less time replenishing each stand. 0.1 to stop idling
    }
    //Properties
    #region PUBLIC PROPERTIES
    [Header("Alchemist Properties")]
    [Header("Personality Properties")]
    public Personality personality = Personality.NORMAL;
    [Tooltip("Seconds needed to take or replenish supplies"), Range(1, 5)]
    public int timeToPrepareMax = 2;
    public int timeToPrepareMin = 2;


    [Header("Efficiency Properties")]
    public Efficiency efficiency = Efficiency.NORMAL;
    [Tooltip("Seconds needed to take or replenish supplies"), Range(1, 5)]
    public int numExtraIngredients = 2;


    [Header("Skill Properties")]
    public Skill skill = Skill.ADEPT;
    [Tooltip("Seconds needed to take or replenish supplies"), Range(1, 5)]
    public int failProbability = 2;


    public int ingredientsAvailable = 10;

    [SerializeField] string stateName;
<<<<<<< Updated upstream
=======
    public Table alchemistTable;
    public Shelf currentShelf;
    public bool newShelf = true;

    public GameObject puddle;

<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    #endregion

    #region PRIVATE PROPERTIES
    StackFiniteStateMachine<Alchemist> alchemistSFSM;
    TextMeshProUGUI serviceText;
    #endregion

    #region STATES
    public FinishPotion_AlchemistState finishingPotionState;
    public Interrupted_AlchemistState interruptedState;
    public PreparingPotion_AlchemistState preparingPotionState;
    public Waiting_AlchemistState waitingState;
    public WaitingIngredients_AlchemistState waitingIngredientsState;
<<<<<<< Updated upstream
=======
    public PickUpIngredients_AlchemistState pickingUpIngredientsState;
    public WaitingForFreeSpace_AlchemistState waitingForSpaceState;

<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    #endregion

    protected override void OnAwake()
    {
        //serviceText = debugCanvas.Find("ServiceText").GetComponent<TextMeshProUGUI>();

        //RandomizeProperties();

<<<<<<< Updated upstream
        //base.OnAwake(); // Sets agent and animator components
=======
        base.OnAwake(); // Sets agent and animator components
       
        alchemistTable.AnnoyingOnTable += OnAnnoyedByCat;
        alchemistTable.AnnoyingOffTable += OnStopAnnoyedByCat;

        personality = (Personality)UnityEngine.Random.Range(0, 3); // Chooses a personality randomly
        efficiency = (Efficiency)UnityEngine.Random.Range(0, 3); // Chooses a personality randomly
        skill = (Skill)UnityEngine.Random.Range(0, 3); // Chooses a personality randomly
<<<<<<< Updated upstream
=======


        switch (personality)
        {
            case Personality.NORMAL:
                speed = 3f;
                timeToPrepareMax = 7;
                timeToPrepareMin = 5;
                break;
            case Personality.LAZY:
                speed = 2f;
                timeToPrepareMax = 15;
                timeToPrepareMin = 7;
                break;
            case Personality.ENERGISED:
                speed = 4f;
                timeToPrepareMax = 4;
                timeToPrepareMin = 1;
                break;
        }

        switch (efficiency)
        {
            case Efficiency.NORMAL:
                numExtraIngredients = UnityEngine.Random.Range(2, 4);
                break;
            case Efficiency.EFFICIENT:
                numExtraIngredients = 1;
                break;
            case Efficiency.INEFFICIENT:
                numExtraIngredients = UnityEngine.Random.Range(4, 8);
                break;
        }

        switch (skill)
        {
            case Skill.NOOB:
                failProbability = 9;   // if(random < failProbability) = fallo
                break;
            case Skill.ADEPT:
                failProbability = 5;
                break;
            case Skill.MASTER:
                failProbability = 2;
                break;
        }
>>>>>>> Stashed changes


        switch (personality)
        {
            case Personality.NORMAL:
                speed = 3f;
                timeToPrepareMax = 7;
                timeToPrepareMin = 5;
                break;
            case Personality.LAZY:
                speed = 2f;
                timeToPrepareMax = 15;
                timeToPrepareMin = 7;
                break;
            case Personality.ENERGISED:
                speed = 4f;
                timeToPrepareMax = 4;
                timeToPrepareMin = 1;
                break;
        }

        switch (efficiency)
        {
            case Efficiency.NORMAL:
                numExtraIngredients = UnityEngine.Random.Range(2, 4);
                break;
            case Efficiency.EFFICIENT:
                numExtraIngredients = 1;
                break;
            case Efficiency.INEFFICIENT:
                numExtraIngredients = UnityEngine.Random.Range(4, 8);
                break;
        }

        switch (skill)
        {
            case Skill.NOOB:
                failProbability = 9;   // if(random < failProbability) = fallo
                break;
            case Skill.ADEPT:
                failProbability = 5;
                break;
            case Skill.MASTER:
                failProbability = 2;
                break;
        }

>>>>>>> Stashed changes
    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {
        // if (stateName != alchemistSFSM.currentState.stateName)
        //     stateName = alchemistSFSM.currentState.stateName;
        CheckCatProximity();

        //if (!HasReachedMaxScares()) ReactToCat();

        //base.OnUpdate(); // No need: Checks animation
    }

    #region PUBLIC METHODS
    /// <returns>If client has been scared enough times</returns>
    /// <summary>
    /// Resets the client's properties and behaviour
    /// </summary>

    public bool HasIngredients()
    {
        return ingredientsAvailable > 0;
    }

    /* Para comprobar si tiene algun pedido
    public bool hasOrder()
    {

    }
    */



    #endregion

    #region PRIVATE	METHODS
    protected override ADecisionSystem<Alchemist> CreateDecisionSystem()
    {
        // Stack Finite State Machine
        alchemistSFSM = new(this);

        // States initialization
        finishingPotionState = new(alchemistSFSM);
        interruptedState = new(alchemistSFSM);
        preparingPotionState = new(alchemistSFSM);
        waitingState = new(alchemistSFSM);
        waitingIngredientsState = new(alchemistSFSM);
<<<<<<< Updated upstream
=======
        pickingUpIngredientsState = new(alchemistSFSM);
        waitingForSpaceState = new(alchemistSFSM);
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

        alchemistSFSM.SetInitialState(waitingState);

        return alchemistSFSM;
    }

    /// <summary>
    /// Checks if the cat is close enough to scare the client
    /// </summary>
    void CheckCatProximity()
    {
        float distanceToCat = Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position);

        if (CatIsBothering())
        {
            alchemistSFSM.SwitchState(interruptedState);
        }
    }

    public override bool CatIsBothering()
    {
        // True if cat is close to the table
        return false;
    }

    #endregion
}
