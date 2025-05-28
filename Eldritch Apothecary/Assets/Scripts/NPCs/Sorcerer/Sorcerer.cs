using System;
using UnityEngine;
using TMPro;

public class Sorcerer : AHumanoid<Sorcerer>
{
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
    public override bool CatIsBothering()
    {
        throw new System.NotImplementedException();
    }
}
