using System;
using UnityEngine;
using TMPro;

public class Sorcerer : AHumanoid<Sorcerer>
{
    public enum Personality
    {
        NORMAL, // Normal speed and time replenishing each stand. 0.2 to stop idling
        LAZY, // Lower speed and more time replenishing each stand. 0.3 to stop idling
        ENERGISED // Higher speed and less time replenishing each stand. 0.1 to stop idling
    }

    #region PUBLIC PROPERTIES
    [Header("Personality Properties")]
    public Personality personality = Personality.NORMAL;
    //[Tooltip("Triggering distance to cat"), Range(0.5f, 5f)]
    //public float minDistanceToCat = 5;
    public GameObject spellVFXPrefab;
    private Transform spellSpawnPoint; // Opcional, para lanzar desde la mano u otra posición

    #endregion    
    #region PRIVATE PROPERTIES

    public StackFiniteStateMachine<Sorcerer> sfsm;
    TextMeshProUGUI _serviceText;
    #endregion

    #region STATES
    public AttendingClients_SorcererState attendingClientsState;
    public Interrupted_SorcererState interruptedState;
    public PickUpIngredients_SorcererState pickUpIngredientsState;
    public WaitForClient_SorcererState waitForClientState;
    public WaitForIngredient_SorcererState waitForIngredientState;
    #endregion

    protected override ADecisionSystem<Sorcerer> CreateDecisionSystem()
    {
        sfsm = new(this);

        attendingClientsState = new(sfsm);
        interruptedState = new(sfsm);
        pickUpIngredientsState = new(sfsm);
        waitForClientState = new(sfsm);
        waitForIngredientState = new(sfsm);

        sfsm.SetInitialState(waitForClientState);

        return sfsm;
    }

    void OnEnable()
    {
        Cat.OnSorcererAnnoyed += OnCatAnnoyedMe;
        Cat.OnSorcererNoLongerAnnoyed += OnCatStoppedAnnoying;
    }

    void OnDisable()
    {
        Cat.OnSorcererAnnoyed -= OnCatAnnoyedMe;
        Cat.OnSorcererNoLongerAnnoyed -= OnCatStoppedAnnoying;
    }

    void OnCatAnnoyedMe()
    {
        if (!(sfsm.Peek() is Interrupted_SorcererState))
        {
            Debug.Log("Cat is bothering me");
            sfsm.PushCurrentState();
            sfsm.SwitchState(interruptedState);
            Debug.Log("El hechicero fue interrumpido por el gato (evento).");
        }
    }

    void OnCatStoppedAnnoying()
    {
        if (sfsm.Peek() is Interrupted_SorcererState)
        {
            sfsm.Pop(); // Volver al estado anterior
            Debug.Log("El gato se ha bajado de la mesa. El hechicero continúa su tarea.");
        }
    }

    public void CastSpellEffect(int spellCastingTime)
    {

        GameObject spawn = new GameObject("SpellSpawnPoint");
        spawn.transform.position = new Vector3(-11, 2, 17);
        spellSpawnPoint = spawn.transform;

        if (spellVFXPrefab != null)
        {
            Vector3 spawnPos = spellSpawnPoint != null ? spellSpawnPoint.position : transform.position;
            Quaternion rotation = spellSpawnPoint != null ? spellSpawnPoint.rotation : Quaternion.identity;

            GameObject vfx = GameObject.Instantiate(spellVFXPrefab, spawnPos, rotation);
            Destroy(vfx, spellCastingTime);
        }

        Destroy(spawn);
        //Debug.Log("Casting spell");
    }


    public override bool CatIsBothering()
    {
        //float currentDistanceToCat = Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position);
        //if (currentDistanceToCat < minDistanceToCat)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return false;
    }
}
