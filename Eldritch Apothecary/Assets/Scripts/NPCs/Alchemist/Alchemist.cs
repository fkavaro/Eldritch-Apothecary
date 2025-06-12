using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Alchemist : AHumanoid<Alchemist>
{
   
    public enum Personality
    {
        NORMAL, // Normal speed and normal time required to prepare a potion
        LAZY, // Lower speed and more time required to prepare a potion
        ENERGISED // Higher speed and less time required to prepare a potion
    }

    public enum Efficiency
    {
        NORMAL, // Normal number of extra ingredients 
        EFFICIENT, // Lower number of extra ingredients 
        INEFFICIENT // Higher number of extra ingredients 
    }
    public enum Skill
    {
        ADEPT, // Normal probability of a potion falling 
        NOOB, // Lower probability of a potion falling 
        MASTER // Higher probability of a potion falling 
    }
    //Properties
    #region PUBLIC PROPERTIES
    [Header("Alchemist Properties")]
    [Header("Personality Properties")]
    public Personality personality = Personality.NORMAL;
    
    public int timeToPrepareMax = 2;
    public int timeToPrepareMin = 2;


    [Header("Efficiency Properties")]
    public Efficiency efficiency = Efficiency.NORMAL;
    
    public int numExtraIngredients = 2;


    [Header("Skill Properties")]
    public Skill skill = Skill.ADEPT;
    
    public int failProbability = 2;

    [SerializeField] string stateName;
    public Table alchemistTable;
    public Shelf currentShelf;
    public bool newShelf = true;

    public GameObject puddle;
    public GameObject cat;

    #endregion

    #region PRIVATE PROPERTIES
    StackFiniteStateMachine<Alchemist> alchemistSFSM;
    TextMeshProUGUI serviceText;

    public bool annoyedByCat = false;
    #endregion

    #region STATES
    public FinishPotion_AlchemistState finishingPotionState;
    public Interrupted_AlchemistState interruptedState;
    public PreparingPotion_AlchemistState preparingPotionState;
    public Waiting_AlchemistState waitingState;
    public WaitingIngredients_AlchemistState waitingIngredientsState;
    public PickUpIngredients_AlchemistState pickingUpIngredientsState;
    public WaitingForFreeSpace_AlchemistState waitingForSpaceState;

    #endregion



    protected override void OnAwake()
    {

        base.OnAwake(); // Sets agent and animator components
       
        alchemistTable.AnnoyingOnTable += OnAnnoyedByCat; //Suscribes to event (triggered when the cat is on the table)
        alchemistTable.AnnoyingOffTable += OnStopAnnoyedByCat; //Suscribes to event (triggered when the cat is on the table)

        personality = (Personality)UnityEngine.Random.Range(0, 3); // Chooses a personality randomly
        efficiency = (Efficiency)UnityEngine.Random.Range(0, 3); // Chooses a efficiency randomly
        skill = (Skill)UnityEngine.Random.Range(0, 3); // Chooses a skill randomly


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
                failProbability = 7;   // if(random < failProbability) = fallo
                break;
            case Skill.ADEPT:
                failProbability = 5;
                break;
            case Skill.MASTER:
                failProbability = 2;
                break;
        }

    }

    private void OnStopAnnoyedByCat(GameObject @object)
    {
        //When the cat goes off the table, the alchemist stops being annoyed
        annoyedByCat = false;
        alchemistSFSM.Pop();
    }

    private void OnAnnoyedByCat(GameObject @object)
    {
        //When the cat goes of the table, the alchemist starts being annoyed
        //If alchemist is using the table
        if (alchemistSFSM.IsCurrentState(preparingPotionState))
        {
            annoyedByCat = true;
            //Changes to interrupted state
            stateName = interruptedState.StateName;
            alchemistSFSM.SwitchState(interruptedState);
        }
    }


    protected override void OnUpdate()
    {

    }


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
        pickingUpIngredientsState = new(alchemistSFSM);
        waitingForSpaceState = new(alchemistSFSM);

        alchemistSFSM.SetInitialState(waitingState);

        return alchemistSFSM;
    }


    public override bool CatIsBothering()
    {
        return annoyedByCat;
    }

    #endregion
}
