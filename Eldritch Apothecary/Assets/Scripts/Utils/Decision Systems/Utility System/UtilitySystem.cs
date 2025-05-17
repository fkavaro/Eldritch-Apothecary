using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Utility System for decision making in agents.
/// </summary>
/// <typeparam name="TController"></typeparam>
public class UtilitySystem<TController> : ADecisionSystem<TController>
where TController : ABehaviourController<TController>
{
    /// <summary>
    /// List of actions available for the agent.
    /// </summary>
    List<IAction> _actions = new();
    IAction _currentAction, _defaultAction; // Only during default action, other actions are checked

    /// <summary>
    /// Dictionary to store the utility of each action.
    /// </summary>
    Dictionary<IAction, float> _actionUtilities = new();

    public UtilitySystem(TController controller) : base(controller) { }

    #region INHERITED METHODS
    protected override void DebugDecision()
    {
        controller.stateText.text = _currentAction.DebugDecision();
    }

    public override void Start()
    {
        Restart();
    }

    public override void Update()
    {
        // Check actions utilities while in default action 
        if (IsCurrentAction(_defaultAction))
            MakeDecision();

        // Update the current action
        _currentAction.UpdateAction();

        // Check if it has finished
        if (_currentAction.IsFinished())
            CurrentAsDefaultAction(); // Reset to default action
    }

    /// <summary>
    /// Resets the utility system according to the current state.
    /// </summary>
    public override void Reset()
    {
        _currentAction.Reset();
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

    /// <summary>
    /// Resets the current action to the default action.
    /// </summary>
    public void CurrentAsDefaultAction()
    {
        if (_defaultAction == null)
        {
            Debug.LogError("Default action is not set. Cannot reset the utility system.");
            return;
        }

        _currentAction = _defaultAction;
    }


    /// <summary>
    /// Restarts the utility system by resetting the current action to the default action and starting it
    /// </summary>
    public void Restart()
    {
        CurrentAsDefaultAction(); // Reset to default action
        _currentAction?.StartAction(); // Start the default action
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
        if (!IsCurrentAction(bestAction))
        {
            // Debug the decision made
            Debug.Log($"{controller.name} decided to: {bestAction.Name} with utility {_actionUtilities[bestAction]}");

            _currentAction = bestAction; // Update current action
            _currentAction.StartAction();
        }

        DebugDecision();

        // Clear the action utilities for the next decision cycle
        _actionUtilities.Clear();
    }


    #endregion
}
