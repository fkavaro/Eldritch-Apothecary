using System;
using System.Collections.Generic;
using UnityEngine;

public class FSM_ClientAction : StateMachineAction<Client, StackFiniteStateMachine<Client>>
{
    readonly StackFiniteStateMachine<Client> _stateMachine;

    #region STATES
    public Shopping_ClientState shoppingState;
    public WaitForReceptionist_ClientState waitForReceptionistState;
    public WaitForService_ClientState waitForServiceState;
    public AtSorcerer_ClientState atSorcererState;
    public TakePotion_ClientState pickPotionUpState;
    public Leaving_ClientState leavingState;
    #endregion

    public FSM_ClientAction(UtilitySystem<Client> utilitySystem, StackFiniteStateMachine<Client> stateMachine) : base(utilitySystem, stateMachine)
    {
        _stateMachine = stateMachine;

        // States initialization
        shoppingState = new(_stateMachine);
        waitForReceptionistState = new(_stateMachine);
        waitForServiceState = new(_stateMachine);
        atSorcererState = new(_stateMachine);
        pickPotionUpState = new(_stateMachine);
        leavingState = new(_stateMachine);

        // Initial state
        // If the client wants to shop, set the shopping state as the initial state
        // There's also a chance to also go shopping although a service is wanted
        if (_controller.wantedService == Client.WantedService.SHOPPING ||
            UnityEngine.Random.Range(0, 11) < 7) // 70% chance
            _stateMachine.SetInitialState(shoppingState);
        else
            _stateMachine.SetInitialState(waitForReceptionistState);
    }
}