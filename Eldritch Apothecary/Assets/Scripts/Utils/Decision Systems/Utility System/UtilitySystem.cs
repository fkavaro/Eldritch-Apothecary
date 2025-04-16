using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UtilitySystem<TController> : ADecisionSystem<TController>
where TController : ABehaviourController<TController>
{
    /// <summary>
    /// List of actions available for the agent.
    /// </summary>
    List<AAction<TController>> _actions = new();
    AAction<TController> _currentAction, _defaultAction;

    /// <summary>
    /// Dictionary to store the utility of each action.
    /// </summary>
    Dictionary<AAction<TController>, float> _actionUtilities = new();

    public UtilitySystem(TController controller) : base(controller) { }

    #region INHERITED METHODS
    protected override void DebugDecision()
    {
        controller.stateText.text = _currentAction.name;
    }


    public override void Start()
    {
        // Invokes the method in time seconds, then repeatedly every repeatRate seconds
        //controller.InvokeRepeating("MakeDecision", 0f, 0.5f);

        //Reset();
    }

    public override void Update()
    {
        // Make a decicision during default action
        if (_currentAction == null || _currentAction == _defaultAction)
        {
            MakeDecision();
        }
        else // Current action is not the default
        {
            _currentAction.UpdateAction(); // Update the current action

            // Check if it has finished
            if (_currentAction.IsFinished())
                Reset();
        }
    }

    public override void Reset()
    {
        _currentAction = null; // ? = defaultAction
    }
    #endregion

    #region PUBLIC METHODS
    public void AddAction(AAction<TController> action, bool isDefault = false)
    {
        if (action == null) return; // Ignore null actions
        if (_actions.Contains(action)) return; // Ignore duplicate actions

        _actions.Add(action);

        if (isDefault)
            SetDefaultAction(action); // Set the default action if specified
    }

    public void SetDefaultAction(AAction<TController> action)
    {
        _defaultAction = action; // Set the new action as default
    }

    public bool IsCurrentAction(AAction<TController> action)
    {
        if (_currentAction == null) return false; // No current action
        return _currentAction == action; // Check if the current action is the same as the given one
    }
    #endregion

    #region PRIVATE METHODS
    void MakeDecision()
    {
        //if (_currentAction != null) return; // Don't interrupt current action

        // Calculate the utility of each available action
        foreach (var action in _actions)
            _actionUtilities.Add(action, action.CalculateUtility());

        // Find the action with the highest utility
        AAction<TController> bestAction = _actionUtilities.OrderByDescending(pair => pair.Value).FirstOrDefault().Key;

        // Execute the best action (if any with positive utility)
        // Assuming the action will handle cleaning 'currentAction' when it finishes (Reset())
        if (bestAction != null && _actionUtilities[bestAction] > 0f)
            _currentAction = bestAction;
        else // If no action has positive utility, use the default action
            _currentAction = _defaultAction;

        _currentAction.StartAction();

        DebugDecision();

        // Clear the action utilities for the next decision cycle
        _actionUtilities.Clear();
    }
    #endregion
}
