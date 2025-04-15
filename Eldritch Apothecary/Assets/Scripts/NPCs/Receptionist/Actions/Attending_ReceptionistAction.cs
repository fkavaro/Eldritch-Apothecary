using UnityEngine;

public class Attending_ReceptionistAction : AAction<Receptionist>
{
    public Attending_ReceptionistAction(UtilitySystem<Receptionist> utilitySystem, bool isDefault = false)
    : base("Attending clients", utilitySystem, isDefault) { }

    public override float CalculateUtility()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {

    }
}
