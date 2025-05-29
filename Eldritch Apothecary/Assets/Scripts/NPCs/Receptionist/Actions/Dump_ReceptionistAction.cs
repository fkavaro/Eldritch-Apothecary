using UnityEngine;
using System.Collections;
using System;

public class Dump_ReceptionistAction : ABinaryAction<Receptionist>
{
    Potion leftPotion;
    Transform dump;
    bool hasTakenPotion,
        hasDumpedPotion;

    public Dump_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem)
    : base("Dumping left potion", utilitySystem, 0.5f) { }

    protected override bool SetDecisionFactor()
    {
        leftPotion = ApothecaryManager.Instance.ALeftPotion();

        return leftPotion != null;
    }

    public override void StartAction()
    {
        hasTakenPotion = false;
        hasDumpedPotion = false;
        dump = ApothecaryManager.Instance.dump;
        _controller.SetDestination(leftPotion.transform.position);
    }

    public override void UpdateAction()
    {
        // Is close to left potion
        if (!hasTakenPotion
            && _controller.IsCloseTo(leftPotion.transform.position, 1))
            // Take it
            _controller.PlayAnimationCertainTime(1f, _controller.pickUpAnim, "Taking potion", TakePotion);
        else if (hasTakenPotion
            && !hasDumpedPotion
            && _controller.IsCloseTo(dump.position, 1))
        {
            // Dump it
            _controller.PlayAnimationCertainTime(1f, _controller.pickUpAnim, "Dumping potion", DumpPotion);
        }
    }

    public override bool IsFinished()
    {
        return hasDumpedPotion;
    }

    void TakePotion()
    {
        leftPotion.Take();
        _controller.SetDestination(dump.position);
        hasTakenPotion = true;
    }

    void DumpPotion()
    {
        ApothecaryManager.Instance.DumpPotion(leftPotion);
        hasDumpedPotion = true;
    }
}