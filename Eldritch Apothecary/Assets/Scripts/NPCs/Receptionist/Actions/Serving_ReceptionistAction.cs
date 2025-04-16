using UnityEngine;

public class Serving_ReceptionistAction : AAction<Receptionist>
{
    public Serving_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem, bool isDefault = false)
    : base("Serving potions", utilitySystem, isDefault) { }

    public override float CalculateUtility()
    {
        // // Check if the receptionist is already serving someone
        // if (controller.IsServing())
        //     return 0f; // No utility if already serving

        // // Check if the receptionist is available to serve
        // if (!controller.IsAvailable())
        //     return 0f; // No utility if not available

        // // Calculate utility based on the number of patients waiting
        // int waitingPatients = controller.GetWaitingPatientsCount();
        // return Mathf.Clamp01(1f - (waitingPatients / 10f)); // Example utility calculation

        // Return number of potions already prepared, ready to be served
        return 0f;
    }

    public override void Execute()
    {
        // Start serving the patient
        // controller.StartServing();
        //_behaviourController.SetTargetSpot(ApothecaryManager.Instance.GetPreparedPotionPos, _behaviourController.pickUpAnim);
    }
}
