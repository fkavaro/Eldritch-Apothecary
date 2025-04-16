using UnityEngine;

/// <summary>
/// Base class for actions that have a binary decision factor (true/false).
/// </summary>
public abstract class ABinaryAction<TController> : AAction<Receptionist, bool> where TController : ABehaviourController<TController>
{
    bool _inverted;

    protected ABinaryAction(string name, UtilitySystem<Receptionist> utilitySystem, bool inverted = false)
    : base(name, utilitySystem)
    {
        _inverted = inverted;
    }

    protected override float CalculateUtility()
    {
        if (_inverted)
        {
            if (_decisionFactor)
                utility = 0f;
            else
                utility = 1f;
        }
        else
        {
            if (_decisionFactor)
                utility = 1f;
            else
                utility = 0f;
        }

        //Debug.Log(name + " utility: " + utility);
        return utility;
    }
}
