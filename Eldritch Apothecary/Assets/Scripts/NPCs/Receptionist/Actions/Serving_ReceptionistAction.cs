using UnityEngine;

public class Serving_ReceptionistAction : ALinearAction<Receptionist>
{
    Potion potionToServe, readyPotion;
    bool hasServed = false, hasTakenPotion;
    int turnNumber;

    public Serving_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem)
    : base("Serving potions", utilitySystem) { }

    protected override float SetDecisionFactor()
    {
        // Return the number of potions already prepared, ready to be served
        // Normalized between 0 and the maximum number of potions that can wait to be served
        return ApothecaryManager.Instance.normalisedPreparedPotions;
    }

    public override void StartAction()
    {
        hasServed = false;
        hasTakenPotion = false;

        // Select a random prepared potion (is assigned)
        potionToServe = ApothecaryManager.Instance.RandomPreparedPotion(true);
        _controller.SetDestination(potionToServe.transform.position);
    }

    public override void UpdateAction()
    {
        // Still hasn't taken the potion to be served and it's close to it
        if (!hasTakenPotion && _controller.IsCloseTo(potionToServe.transform.position, 1))
            // Take it
            _controller.PlayAnimationCertainTime(1f, _controller.pickUpAnim, "Taking potion", TakePotion);
        // Has taken the potion to be served and it's close to the ready potion
        else if (hasTakenPotion
            && !hasServed
            && _controller.IsCloseTo(readyPotion.transform.position, 1))
            // Serve it
            _controller.PlayAnimationCertainTime(1f, _controller.pickUpAnim, "Serving potion", PutPotion);
    }

    public override bool IsFinished()
    {
        return hasServed;
    }

    void TakePotion()
    {
        turnNumber = potionToServe.Take();
        readyPotion = ApothecaryManager.Instance.RandomReadyPotion();
        _controller.SetDestination(readyPotion.transform.position);
        hasTakenPotion = true;
    }

    void PutPotion()
    {
        hasServed = true;
        readyPotion.Assign(turnNumber);
    }
}
