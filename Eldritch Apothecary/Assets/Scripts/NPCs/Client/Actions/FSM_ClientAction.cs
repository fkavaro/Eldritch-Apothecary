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
    public Robbing_ClientState robbingState;
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
        robbingState = new(_stateMachine);

        int rndInitialState = UnityEngine.Random.Range(0, 11);

        // Starts shopping
        if (_controller.wantedService == Client.WantedService.SHOPPING || // Only wants to shop
            _controller.personality == Client.Personality.SHOPLIFTER || // Is a SHOPLIFTER
            rndInitialState < 7) // random 70% chance
            _stateMachine.SetInitialState(shoppingState);
        // Directly waits for receptionist
        else
            _stateMachine.SetInitialState(waitForReceptionistState);
    }
}