using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Sorcerer class that represents a client in the game.
/// </summary>
public class Sorcerer : AHumanoid<Sorcerer>
{
    public enum Personality
    {
        NORMAL, // Normal speed and time casting spells and moving around
        LAZY, // Lower speed and more time casting spells and moving around
        ENERGISED // Higher speed and less casting spells and moving around
    }

    public enum Efficiency
    {
        NORMAL, // Normal amount of ingredients used for spells
        EFFICIENT, // Lower amount of ingredients used for spells
        INEFFICIENT, // Higher amount of ingredients used for spells
    }
    public enum Skill
    {
        NOVICE, // Higher chance of failing a spell
        ADEPT, // Normal chance of failing a spell
        MASTER // Lower chance of failing a spell
    }

    #region PUBLIC PROPERTIES
    [Header("Personality Properties")]
    public Personality personality = Personality.NORMAL;
    public Efficiency efficiency = Efficiency.NORMAL;
    public Skill skill = Skill.ADEPT;
    public bool annoyedByCat = false;
    [SerializeField] public GameObject[] spellVFXPrefabs;
    #endregion    

    #region PRIVATE PROPERTIES
    Table sorcererTable;
    public StackFiniteStateMachine<Sorcerer> sfsm;
    private Transform spellSpawnPoint;
    public Transform spellPos;
    #endregion

    #region STATES
    public AttendingClients_SorcererState attendingClientsState;
    public Interrupted_SorcererState interruptedState;
    public PickUpIngredients_SorcererState pickUpIngredientsState;
    public WaitForClient_SorcererState waitForClientState;
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Sorcerer> CreateDecisionSystem()
    {
        // Stack Finite State Machine
        sfsm = new(this);

        attendingClientsState = new(sfsm);
        interruptedState = new(sfsm);
        pickUpIngredientsState = new(sfsm);
        waitForClientState = new(sfsm);

        sfsm.SetInitialState(waitForClientState);

        return sfsm;
    }

    /// <summary>
    /// Randomizes sorcerer properties: pertsonality, efficiency and skill level
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();

        sorcererTable = ApothecaryManager.Instance.sorcererTable;

        sorcererTable.AnnoyingOnTable += OnCatAnnoyedMe; //Suscribes to event (triggered when the cat is on the table)
        sorcererTable.AnnoyingOffTable += OnCatStoppedAnnoying; //Suscribes to event (triggered when the cat is off the table)


        personality = (Personality)UnityEngine.Random.Range(0, 3);
        efficiency = (Efficiency)UnityEngine.Random.Range(0, 3);
        skill = (Skill)UnityEngine.Random.Range(0, 3);

        switch (personality)
        {
            case Personality.NORMAL:
                speed = 3f;
                break;
            case Personality.LAZY:
                speed = 2f;
                break;
            case Personality.ENERGISED:
                speed = 4f;
                break;
        }
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Spawns a random spell visual effect from the list, plays it for a set duration
    /// </summary>
    public void CastSpellEffect(int spellCastingTime)
    {
        GameObject spawn = new("SpellSpawnPoint");
        spawn.transform.position = spellPos.position;
        spellSpawnPoint = spawn.transform;

        if (spellVFXPrefabs != null && spellVFXPrefabs.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, spellVFXPrefabs.Length);
            GameObject selectedVFX = spellVFXPrefabs[index];

            Vector3 spawnPos = spellSpawnPoint.position;
            Quaternion rotation = spellSpawnPoint.rotation;

            GameObject vfx = GameObject.Instantiate(selectedVFX, spawnPos, rotation);
            Destroy(vfx, spellCastingTime);
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
    /// <summary>
    /// Saves the current state and switches to the interrupted state
    /// </summary>
    void OnCatAnnoyedMe(GameObject @object)
    {
        if (!sfsm.IsCurrentState(interruptedState))
        {
            if (debugMode) Debug.Log("Cat is bothering me");
            annoyedByCat = true;
            sfsm.PushCurrentState();
            sfsm.SwitchState(interruptedState);
        }
    }

    /// <summary>
    /// Pops the state stack to resume previous behavior
    /// </summary>
    void OnCatStoppedAnnoying(GameObject @object)
    {
        if (sfsm.IsCurrentState(interruptedState))
        {
            annoyedByCat = false;
            sfsm.Pop();
            if (debugMode) Debug.Log("Cat is no longer bothering me. Continuing my tasks");
        }
    }
    #endregion
}
