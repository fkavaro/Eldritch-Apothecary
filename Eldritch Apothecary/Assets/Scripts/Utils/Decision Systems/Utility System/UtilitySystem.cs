using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    protected override void DebugDecision()
    {
        controller.stateText.text = _currentAction.name;
    }


    public override void Start()
    {
        // Invokes the method in time seconds, then repeatedly every repeatRate seconds
        //controller.InvokeRepeating("MakeDecision", 0f, 0.5f);
    }

    public override void Update()
    {
        // Logic for updating the decision system (if needed)
        if (_currentAction == null)
        {
            MakeDecision();
        }
    }

    public override void Reset()
    {
        _currentAction = null;
    }

    public void AddAction(AAction<TController> action, bool isDefault = false)
    {
        if (action == null) return; // Ignore null actions
        if (_actions.Contains(action)) return; // Ignore duplicate actions

        _actions.Add(action);

        if (isDefault)
            _defaultAction = action; // Set the new action as default
    }

    void MakeDecision()
    {
        if (_currentAction != null) return; // Don't interrupt current action

        // Calculate the utility of each available action
        foreach (var action in _actions)
            _actionUtilities.Add(action, action.CalculateUtility(controller));

        // Find the action with the highest utility
        AAction<TController> bestAction = _actionUtilities.OrderByDescending(pair => pair.Value).FirstOrDefault().Key;

        // Execute the best action (if any with positive utility)
        // Assuming the action will handle cleaning 'currentAction' when it finishes (Reset())
        if (bestAction != null && _actionUtilities[bestAction] > 0f)
            _currentAction = bestAction;
        else // If no action has positive utility, use the default action
            _currentAction = _defaultAction;

        _currentAction.Execute(controller);

        DebugDecision();

        // Clear the action utilities for the next decision cycle
        _actionUtilities.Clear();
    }
}
