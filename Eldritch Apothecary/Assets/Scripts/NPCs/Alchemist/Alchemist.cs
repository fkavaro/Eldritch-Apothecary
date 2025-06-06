using System;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Alchemist : AHumanoid<Alchemist>
{

    //Properties
    #region PUBLIC PROPERTIES
    [Header("Alchemist Properties")]
    [Tooltip("Number of available ingredients"), Range(0f, 4f)]
    public int ingredientsAvailable = 10;
    [SerializeField] string stateName;
    public Table alchemistTable;
    public Shelf currentShelf;
    public bool newShelf = true;
    #endregion

    #region PRIVATE PROPERTIES
    StackFiniteStateMachine<Alchemist> alchemistSFSM;
    TextMeshProUGUI serviceText;

    private bool annoyedByCat = false;
    #endregion

    #region STATES
    public FinishPotion_AlchemistState finishingPotionState;
    public Interrupted_AlchemistState interruptedState;
    public PreparingPotion_AlchemistState preparingPotionState;
    public Waiting_AlchemistState waitingState;
    public WaitingIngredients_AlchemistState waitingIngredientsState;
    public PickUpIngredients_AlchemistState pickingUpIngredientsState;
    #endregion



    protected override void OnAwake()
    {
        //serviceText = debugCanvas.Find("ServiceText").GetComponent<TextMeshProUGUI>();

        //RandomizeProperties();

        base.OnAwake(); // Sets agent and animator components
       
        alchemistTable.AnnoyingOnTable += OnAnnoyedByCat;
        alchemistTable.AnnoyingOffTable += OnStopAnnoyedByCat;


    }

    private void OnStopAnnoyedByCat(GameObject @object)
    {
        annoyedByCat = false;
        alchemistSFSM.Pop();
    }

    private void OnAnnoyedByCat(GameObject @object)
    {
        //@object = GO_cat.gameObject;
        if (alchemistSFSM.IsCurrentState(preparingPotionState))
        {
            Debug.Log("Alchemist fue molestado por: " + @object.name);
            annoyedByCat = true;
            stateName = interruptedState.StateName;
            alchemistSFSM.SwitchState(interruptedState);
        }
    }


    protected override void OnUpdate()
    {
        // if (stateName != alchemistSFSM.currentState.stateName)
        //     stateName = alchemistSFSM.currentState.stateName;
        //CheckCatProximity();

        //if (!HasReachedMaxScares()) ReactToCat();

        //base.OnUpdate(); // No need: Checks animation
    }

    #region PUBLIC METHODS
    /// <returns>If client has been scared enough times</returns>
    /// <summary>
    /// Resets the client's properties and behaviour
    /// </summary>
    /*
    public bool HasIngredients()
    {
        return ingredientsAvailable > 0;
    }
    */
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
        pickingUpIngredientsState = new(alchemistSFSM);

        alchemistSFSM.SetInitialState(waitingState);

        return alchemistSFSM;
    }


    /// <summary>
    /// Checks if the cat is close enough to scare the client
    /// </summary>
    /*void CheckCatProximity()
    {
        float distanceToCat = Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position);

        if (CatIsBothering())
        {
            alchemistSFSM.SwitchState(interruptedState);
        }
    }*/

    public override bool CatIsBothering()
    {
        return annoyedByCat;
    }

    #endregion
}
