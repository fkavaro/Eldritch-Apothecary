using UnityEngine;

public class Serving_ReceptionistAction : ALinearAction<Receptionist>
{
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
        // Start serving the patient
        // controller.StartServing();
        //_behaviourController.SetTargetSpot(ApothecaryManager.Instance.GetPreparedPotionPos, _behaviourController.pickUpAnim);
    }

    public override void UpdateAction()
    {

    }

    public override bool IsFinished()
    {
        // True if the taken potion has been served
        throw new System.NotImplementedException();
    }
}
