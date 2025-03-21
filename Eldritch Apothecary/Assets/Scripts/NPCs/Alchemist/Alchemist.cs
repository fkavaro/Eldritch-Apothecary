using System;
using TMPro;
using UnityEngine;

public class Alchemist : AHumanoid
{

    //Properties
    #region PUBLIC PROPERTIES
    [Header("Alchemist Properties")]
    
    [Tooltip("Triggering distance to cat"), Range(0f, 4f)]
    public float minDistanceToCat = 3f;

    [Tooltip("Number of available ingredients"), Range(0f, 4f)]
    public int ingredientsAvailable = 10;

    [SerializeField] string stateName; 
    #endregion

    #region PRIVATE PROPERTIES
    StackFiniteStateMachine alchemistSFSM;
    TextMeshProUGUI serviceText;
    #endregion

    #region STATES
    public FinishPotion_AlchemistState finishingPotionState;
    public Interrupted_AlchemistState interruptedState;
    public PreparingPotion_AlchemistState preparingPotionState;
    public Waiting_AlchemistState waitingState;
    public WaitingIngredients_AlchemistState waitingIngredientsState;
       #endregion

    protected override void OnAwake()
    {
        //serviceText = debugCanvas.Find("ServiceText").GetComponent<TextMeshProUGUI>();

        //RandomizeProperties();

        //base.OnAwake(); // Sets agent and animator components
    }

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {
        if (stateName != alchemistSFSM.currentState.stateName)
            stateName = alchemistSFSM.currentState.stateName;
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
    protected override ADecisionSystem CreateDecisionSystem()
    {
        // Stack Finite State Machine
        alchemistSFSM = new(this);

        // States initialization
        finishingPotionState = new(alchemistSFSM, this);
        interruptedState = new(alchemistSFSM, this);
        preparingPotionState = new(alchemistSFSM, this);
        waitingState = new(alchemistSFSM, this);
        waitingIngredientsState = new(alchemistSFSM, this);
        
        alchemistSFSM.SetInitialState(waitingState);
       
        return alchemistSFSM;
    }

    /// <summary>
    /// Checks if the cat is close enough to scare the client
    /// </summary>
    void CheckCatProximity()
    {
        float distanceToCat = Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position);

        if (distanceToCat < minDistanceToCat)
        {
            alchemistSFSM.SwitchState(interruptedState);
        }
    }

    #endregion
}
