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

    public GameObject spellVFXPrefab;
    #endregion    

    #region PRIVATE PROPERTIES

    public StackFiniteStateMachine<Sorcerer> sfsm;
    TextMeshProUGUI _serviceText;
    private Transform spellSpawnPoint;
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
    /// Spawns a temporary spell visual effect for a certain time, and then destroys it
    /// </summary>
    public void CastSpellEffect(int spellCastingTime)
    {

        GameObject spawn = new GameObject("SpellSpawnPoint");
        spawn.transform.position = new Vector3(-11, 2, 17);
        spellSpawnPoint = spawn.transform;

        if (spellVFXPrefab != null)
        {
            spellVFXPrefab.SetActive(true);
            Vector3 spawnPos = spellSpawnPoint != null ? spellSpawnPoint.position : transform.position;
            Quaternion rotation = spellSpawnPoint != null ? spellSpawnPoint.rotation : Quaternion.identity;

            GameObject vfx = GameObject.Instantiate(spellVFXPrefab, spawnPos, rotation);
            Destroy(vfx, spellCastingTime);
        }

        spellVFXPrefab.SetActive(false);
        Destroy(spawn);
        if (debugMode) Debug.Log("Casting spell");
    }

    /// <summary>
    /// Determines if the cat is currently bothering the sorcerer 
    /// Always returns false in this implementation
    /// </summary>
    public override bool CatIsBothering()
    {
        return false;
    }
    #endregion

    #region PRIVATE METHODS
    /// <summary>
    /// Subscribes the client to cat annoyance events when the sorcerer is enabled
    /// </summary>
    void OnEnable()
    {
        Cat.OnSorcererAnnoyed += OnCatAnnoyedMe;
        Cat.OnSorcererNoLongerAnnoyed += OnCatStoppedAnnoying;
    }

    /// <summary>
    /// Unsubscribes the client from cat annoyance events when the sorcerer is disabled
    /// </summary>
    void OnDisable()
    {
        Cat.OnSorcererAnnoyed -= OnCatAnnoyedMe;
        Cat.OnSorcererNoLongerAnnoyed -= OnCatStoppedAnnoying;
    }

    /// <summary>
    /// Saves the current state and switches to the interrupted state
    /// </summary>
    void OnCatAnnoyedMe()
    {
        if (!(sfsm.Peek() is Interrupted_SorcererState))
        {
            if (debugMode) Debug.Log("Cat is bothering me");
            sfsm.PushCurrentState();
            sfsm.SwitchState(interruptedState);
        }
    }

    /// <summary>
    /// Pops the state stack to resume previous behavior
    /// </summary>
    void OnCatStoppedAnnoying()
    {
        if (sfsm.Peek() is Interrupted_SorcererState)
        {
            sfsm.Pop();
            if (debugMode) Debug.Log("Cat is no longer bothering me. Continuing my tasks");
        }
    } 
    #endregion
}
