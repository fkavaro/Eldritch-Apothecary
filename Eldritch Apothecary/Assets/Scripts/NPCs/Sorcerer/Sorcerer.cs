using System;
using UnityEngine;
using TMPro;

public class Sorcerer : AHumanoid<Sorcerer>
{
    #region PRIVATE PROPERTIES

    StackFiniteStateMachine<Sorcerer> _sorcererSFSM;
    //UtilitySystem<Client> _clientUS;
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
        _sorcererSFSM = new(this);

        attendingClientsState = new(_sorcererSFSM);
        interruptedState = new(_sorcererSFSM);
        pickUpIngredientsState = new(_sorcererSFSM);
        waitForClientState = new(_sorcererSFSM);
        waitForIngredientState = new(_sorcererSFSM);

        _sorcererSFSM.SetInitialState(attendingClientsState);
        Debug.Log("Hechicero creado");

        return _sorcererSFSM;
    }
    public override bool CatIsBothering()
    {
        throw new System.NotImplementedException();
    }
}
