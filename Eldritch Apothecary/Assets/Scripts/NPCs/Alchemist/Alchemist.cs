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
    public bool annoyedByCat = false;
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

    public GameObject[] potionVFXPrefabs;
    #endregion

    #region PRIVATE PROPERTIES
    StackFiniteStateMachine<Alchemist> alchemistSFSM;
    Table alchemistTable;
    private Transform preparePotionSpawnPoint;
    #endregion

    #region STATES
    public FinishPotion_AlchemistState finishingPotionState;
    public Interrupted_AlchemistState interruptedState;
    public PreparingPotion_AlchemistState preparingPotionState;
    public Waiting_AlchemistState waitingState;
    public PickUpIngredients_AlchemistState pickingUpIngredientsState;
    public WaitingForFreeSpace_AlchemistState waitingForSpaceState;
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Alchemist> CreateDecisionSystem()
    {
        // Stack Finite State Machine
        alchemistSFSM = new(this);

        // States initialization
        finishingPotionState = new(alchemistSFSM);
        interruptedState = new(alchemistSFSM);
        preparingPotionState = new(alchemistSFSM);
        waitingState = new(alchemistSFSM);
        pickingUpIngredientsState = new(alchemistSFSM);
        waitingForSpaceState = new(alchemistSFSM);

        alchemistSFSM.SetInitialState(waitingState);

        return alchemistSFSM;
    }

    protected override void OnAwake()
    {

        base.OnAwake(); // Sets agent and animator components

        alchemistTable = ApothecaryManager.Instance.alchemistTable;

        alchemistTable.AnnoyingOnTable += OnAnnoyedByCat; //Suscribes to event (triggered when the cat is on the table)
        alchemistTable.AnnoyingOffTable += OnStopAnnoyedByCat; //Suscribes to event (triggered when the cat is off the table)

        personality = (Personality)UnityEngine.Random.Range(0, 3); // Chooses a personality randomly
        efficiency = (Efficiency)UnityEngine.Random.Range(0, 3); // Chooses a efficiency randomly
        skill = (Skill)UnityEngine.Random.Range(0, 3); // Chooses a skill randomly

        switch (personality)
        {
            case Personality.NORMAL:
                speed = 3f;
                timeToPrepareMax = 14;
                timeToPrepareMin = 10;
                break;
            case Personality.LAZY:
                speed = 2f;
                timeToPrepareMax = 15;
                timeToPrepareMin = 20;
                break;
            case Personality.ENERGISED:
                speed = 4f;
                timeToPrepareMax = 7;
                timeToPrepareMin = 10;
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
                failProbability = 3;
                break;
            case Skill.ADEPT:
                failProbability = 2;
                break;
            case Skill.MASTER:
                failProbability = 1;
                break;
        }
    }
    #endregion

    #region PUBLIC METHODS
    public void PreparePotionEffect(int spellCastingTime)
    {
        GameObject spawn = new("PreparePotionSpawnPoint");
        spawn.transform.position = new(1, 1.5f, 11);
        preparePotionSpawnPoint = spawn.transform;

        if (potionVFXPrefabs != null && potionVFXPrefabs.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, potionVFXPrefabs.Length);
            GameObject selectedVFX = potionVFXPrefabs[index];

            Vector3 spawnPos = preparePotionSpawnPoint.position;
            Quaternion rotation = preparePotionSpawnPoint.rotation;

            GameObject vfx = GameObject.Instantiate(selectedVFX, spawnPos, rotation);
            Destroy(vfx, spellCastingTime);
        }

        Destroy(spawn);

        if (debugMode) Debug.Log("Casting spell");
    }

    public void FallenPotionEffect(Vector3 puddlePos)
    {
        GameObject spawn = new GameObject("FallenPotionSpawnPoint");
        spawn.transform.position = puddlePos;
        preparePotionSpawnPoint = spawn.transform;

        if (potionVFXPrefabs != null && potionVFXPrefabs.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, potionVFXPrefabs.Length);
            GameObject selectedVFX = potionVFXPrefabs[index];

            Vector3 spawnPos = preparePotionSpawnPoint.position;
            Quaternion rotation = preparePotionSpawnPoint.rotation;

            GameObject vfx = GameObject.Instantiate(selectedVFX, spawnPos, rotation);
            Destroy(vfx, 1);
        }
        Destroy(spawn);

        if (debugMode) Debug.Log("Casting spell");
    }

    public override bool CatIsBothering()
    {
        return annoyedByCat;
    }
    #endregion

    #region PRIVATE METHODS
    void OnAnnoyedByCat(GameObject @object)
    {
        //When the cat goes on the table, the alchemist starts being annoyed
        if (!alchemistSFSM.IsCurrentState(interruptedState) && !alchemistSFSM.IsCurrentState(finishingPotionState))
        {
            if (debugMode) Debug.Log("Cat is bothering" + name);
            annoyedByCat = true;
            alchemistSFSM.PushCurrentState();
            alchemistSFSM.SwitchState(interruptedState);
        }
    }

    void OnStopAnnoyedByCat(GameObject @object)
    {
        if (alchemistSFSM.IsCurrentState(interruptedState))
        {
            //When the cat goes off the table, the alchemist stops being annoyed
            annoyedByCat = false;
            alchemistSFSM.Pop();
            if (debugMode) Debug.Log("Cat is no longer bothering " + name + ". Continuing my tasks");
        }
    }
    #endregion
}
