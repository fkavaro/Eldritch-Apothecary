using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Client class that represents a client in the game.
/// </summary>
public class Client : AHumanoid<Client>
{
    public enum WantedService
    {
        SHOPPING,
        SPELL,
        POTION
    }

    public enum Personality
    {
        NORMAL, // More wait time and less fear
        IMPATIENT, // Less wait time and mid fear
        SKITTISH, // Less wait time and more fear
        SHOPLIFTER // Will steal shop goods
    }

    #region PUBLIC PROPERTIES
    [Header("Wanted Service Properties")]
    [Tooltip("Desired service to be attended")]
    public WantedService wantedService;
    [Tooltip("Turn number assigned to this client")]
    public int turnNumber = -1;

    [Header("Personality Properties")]
    public Personality personality = Personality.NORMAL;
    [Tooltip("Normalized between 0 and the maximum waiting time"), Range(0f, 1f)]
    public float normalisedWaitingTime;
    [Tooltip("Maximum waiting minutes"), Range(1, 3)]
    public int maxMinutesWaiting = 3;
    [Tooltip("Seconds waiting first in line")]
    public float secondsWaiting = 0f;
    [Tooltip("Probability of being scared"), Range(0, 10)]
    public int fear = 1;
    [Tooltip("Triggering distance to cat"), Range(0.5f, 2f)]
    public float minDistanceToCat = 2f;
    [Tooltip("Minimum seconds between scares"), Range(10f, 120f)]
    public float minSecondsBetweenScares = 30f;
    [Tooltip("Maximum number of scares supported"), Range(1, 5)]
    public int maxScares = 2;
    public int scaresCount = 0;

    [HideInInspector] public float lastScareTime = -Mathf.Infinity;
    [HideInInspector] public TextMeshProUGUI turnText;

    // Renderes from the game object to change material colors
    Renderer[] renderers;
    #endregion

    #region PRIVATE PROPERTIES
    StackFiniteStateMachine<Client> _sfsm;
    UtilitySystem<Client> _us;
    TextMeshProUGUI _serviceText;
    #endregion

    #region ACTIONS
    public FSM_ClientAction fsmAction;
    public StunnedByCat_ClientAction stunnedByCatAction;
    public Complain_ClientAction complainAction;
    #endregion

    #region INHERITED METHODS
    protected override ADecisionSystem<Client> CreateDecisionSystem()
    {
        // Finite State Machine
        _sfsm = new(this);
        // Utility System
        _us = new(this);

        // Action
        fsmAction = new(_us, _sfsm);
        stunnedByCatAction = new(_us);
        complainAction = new(_us);

        return _us;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        _serviceText = debugCanvas.Find("Service").GetComponent<TextMeshProUGUI>();
        turnText = debugCanvas.Find("Turn").GetComponent<TextMeshProUGUI>();

        // Random navMeshAgent priority
        _agent.avoidancePriority = UnityEngine.Random.Range(10, 99);
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();

        // Normalise time between 0 and the 1 as the maximum waiting time of the client
        normalisedWaitingTime = Mathf.Clamp01(secondsWaiting / (maxMinutesWaiting * 60f));
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Resets the client's properties and behaviour
    /// </summary>
    public void Reset()
    {
        RandomizeProperties();
        SetIfStopped(false); // ANPC
        ResetBehaviour(); // ABehaviourController
    }

    /// <returns> True if the client has waited too long, false otherwise.</returns>
    public bool WaitedTooLong()
    {
        return secondsWaiting >= maxMinutesWaiting * 60f;
    }

    /// <summary>
    /// Client is not affected by anything
    /// </summary>
    public void DontMindAnything()
    {
        scaresCount = 0;
        secondsWaiting = 0f;
        normalisedWaitingTime = 0f;
        fear = 0;
        isExecutionPaused = false;
    }

    public override bool CatIsBothering()
    {
        float currentDistanceToCat = Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position);
        bool enoughTimeSinceLastScare = (Time.time - lastScareTime) >= minSecondsBetweenScares;

        if (currentDistanceToCat < minDistanceToCat // Cat is close
            && enoughTimeSinceLastScare // Enough time has passed since last scare
            && _us.IsCurrentAction(fsmAction) // Executing SFSM (not stunned nor complaining)
            && !_sfsm.IsCurrentState(fsmAction.leavingState) // Not leaving
            && !ApothecaryManager.Instance.waitingQueue.Contains(this) // Not in waiting queue
            && UnityEngine.Random.Range(0, 10) < fear // Checks scare probability
        )
            return true;
        else
            return false;
    }

    /// <returns>If client has been scared enough times</returns>
    public bool TooScared()
    {
        return scaresCount >= maxScares;
    }

    /// <summary>
    /// Makes client bigger when spell goes wrong
    /// </summary>
    public void Enlarge()
    {
        transform.localScale *= 1.2f;
    }

    /// <summary>
    /// Makes client smaller when spell goes wrong
    /// </summary>
    public void Shrink()
    {
        transform.localScale *= 0.8f;
    }

    /// <summary>
    /// Resets client scale
    /// </summary>
    public void ResetScale()
    {
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Changes client color when spell goes wrong
    /// </summary>
    public void ChangeColor()
    {
        // Accesses renderers in game object
        renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                // Changes the skin to a random color
                if (materials[i].name.Contains("Skin"))
                {
                    materials[i].color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                }
            }
            renderer.materials = materials;
        }
    }

    /// <summary>
    /// Resets client skin color
    /// </summary>
    public void ResetColor()
    {
        renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i].name.Contains("Skin"))
                {
                    materials[i].color = new Color(233f / 255f, 200f / 255f, 173f / 255f, 255f / 255f);
                }
            }
            renderer.materials = materials;
        }
    }
    #endregion

    #region PRIVATE	METHODS
    /// <summary>
    /// Randomizes client properties: wanted service, max minutes waiting, scare probability and max scares
    /// </summary>
    void RandomizeProperties()
    {
        wantedService = (WantedService)UnityEngine.Random.Range(0, 3); // Chooses a service randomly

        float rndPersonality = UnityEngine.Random.Range(0, 11);

        if (rndPersonality <= 0.5) // 10% chance
            personality = Personality.SHOPLIFTER;
        else if (rndPersonality > 0.5 && rndPersonality <= 4) // 30% chance
            personality = Personality.IMPATIENT;
        else if (rndPersonality > 4 && rndPersonality <= 7) // 30% chance
            personality = Personality.SKITTISH;
        else if (rndPersonality > 7 && rndPersonality <= 10) // 30% chance
            personality = Personality.NORMAL;

        switch (personality)
        {
            case Personality.NORMAL:
                speed = UnityEngine.Random.Range(2, 4);
                maxMinutesWaiting = 3;
                fear = UnityEngine.Random.Range(0, 3);
                minDistanceToCat = 0.5f;
                minSecondsBetweenScares = 120f;
                maxScares = UnityEngine.Random.Range(4, 6);
                break;
            case Personality.IMPATIENT:
                speed = UnityEngine.Random.Range(3, 5);
                maxMinutesWaiting = 1;
                fear = UnityEngine.Random.Range(3, 6);
                minDistanceToCat = 1f;
                minSecondsBetweenScares = 60f;
                maxScares = UnityEngine.Random.Range(2, 4);
                break;
            case Personality.SHOPLIFTER: // Like impatient but is not affected by anything
                speed = UnityEngine.Random.Range(3, 5);
                maxMinutesWaiting = 1;
                fear = UnityEngine.Random.Range(3, 6);
                minDistanceToCat = 1f;
                minSecondsBetweenScares = 60f;
                maxScares = UnityEngine.Random.Range(2, 4);
                DontMindAnything();
                wantedService = WantedService.SHOPPING; // Only wants shop goods
                break;
            case Personality.SKITTISH:
                speed = UnityEngine.Random.Range(2, 4);
                maxMinutesWaiting = 2;
                fear = UnityEngine.Random.Range(6, 11);
                minDistanceToCat = 2f;
                minSecondsBetweenScares = 30f;
                maxScares = UnityEngine.Random.Range(1, 3);
                break;
        }

        _serviceText.text = wantedService.ToString();
    }

    public void ResetWaitingTime()
    {
        secondsWaiting = 0f;
        normalisedWaitingTime = 0f;
    }
    #endregion
}