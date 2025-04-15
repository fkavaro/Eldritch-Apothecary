using UnityEngine;

public class CalmingDown_ReceptionistAction : AAction<Receptionist>
{
    public CalmingDown_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem, bool isDefault = false)
    : base("Calming down a client", utilitySystem, isDefault) { }

    public override float CalculateUtility()
    {
        // // Check if the receptionist is in a state where they can calm down
        // if (controller.attendingState.IsInState() && controller.attendingState.IsBusy())
        // {
        //     return 0.5f; // Medium utility to calm down
        // }
        // return 0f; // No utility if not in a valid state
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        // Logic to calm down the receptionist
        // controller.attendingState.SetCalm(true);
    }
}
