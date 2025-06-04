using System;
using UnityEngine;
using TMPro;

public class Sorcerer : AHumanoid<Sorcerer>
{
    #region PUBLIC PROPERTIES
    [Tooltip("Triggering distance to cat"), Range(0.5f, 5f)]
    public float minDistanceToCat = 5;

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


    public override bool CatIsBothering()
    {
        float currentDistanceToCat = Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position);
        if (currentDistanceToCat < minDistanceToCat)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
