using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

public class UtilitySystem<TController> : ADecisionSystem<TController>
where TController : ABehaviourController<TController>
{
    /// <summary>
    /// List of actions available for the agent.
    /// </summary>
    List<IAction> _actions = new();
    IAction _currentAction, _defaultAction;

    /// <summary>
    /// Dictionary to store the utility of each action.
    /// </summary>
    Dictionary<IAction, float> _actionUtilities = new();

    public UtilitySystem(TController controller) : base(controller) { }

    #region INHERITED METHODS
    protected override void DebugDecision()
    {
        controller.stateText.text = _currentAction.Name;
    }


    public override void Start()
    {
        Reset();
    }

    public override void Update()
    {
        // Check actions utilities while in default action 
        if (_currentAction == _defaultAction)
            MakeDecision();

        // Update the current action
        _currentAction.UpdateAction();

        // Check if it has finished
        if (_currentAction.IsFinished())
            Reset();
    }

    public override void Reset()
    {
        if (_defaultAction == null)
        {
            Debug.LogError("Default action is not set. Cannot reset the utility system.");
            return;
        }

        _currentAction = _defaultAction;
        _currentAction?.StartAction(); // Start the default action
    }
    #endregion

    #region PUBLIC METHODS
    public void AddAction(IAction action)
    {
        if (action == null) return; // Ignore null actions
        if (_actions.Contains(action)) return; // Ignore duplicate actions

        _actions.Add(action);
    }

    public void SetDefaultAction(IAction action)
    {
        _defaultAction = action; // Set the new action as default
    }

    public bool IsCurrentAction(IAction action)
    {
        if (_currentAction == null) return false; // No current action
        return _currentAction == action; // Check if the current action is the same as the given one
    }
    #endregion

    #region PRIVATE METHODS
    void MakeDecision()
    {
        //Debug.Log(controller.name + " making decision...");

        // Calculate the utility of each available action
        foreach (var action in _actions)
            _actionUtilities.Add(action, action.Utility);

        // Find the action with the highest utility
        IAction bestAction = _actionUtilities.OrderByDescending(pair => pair.Value).FirstOrDefault().Key;

        // If the best action has negative utility, use the default action
        if (_actionUtilities[bestAction] < 0f || bestAction == null)
        {
            Debug.LogError($"Best action is null or has negative utility, using default action: {_defaultAction.Name}");
            bestAction = _defaultAction;
        }

        // Start the best action if it's different from the current one
        if (bestAction != _currentAction)
        {
            _currentAction = bestAction;
            _currentAction.StartAction();

            // Debug the decision made
            //Debug.Log($"Decision made: {_currentAction.Name} with utility {_actionUtilities[_currentAction]}");
        }

        DebugDecision();

        // Clear the action utilities for the next decision cycle
        _actionUtilities.Clear();
    }
    #endregion
}
