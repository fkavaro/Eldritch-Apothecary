using UnityEngine;

public abstract class AHumanoid : ANPC
{
    #region HUMANOID ANIMATIONS
    readonly public int Talk = Animator.StringToHash("Talk"),
        PickUp = Animator.StringToHash("PickUp"),
        Complain = Animator.StringToHash("Complain"),
        Stunned = Animator.StringToHash("Stunned"),
        SitDown = Animator.StringToHash("SitDown"),
        StandUp = Animator.StringToHash("StandUp"),
        Argue = Animator.StringToHash("Argue"),
        Yell = Animator.StringToHash("Yell"),
        CastSpell = Animator.StringToHash("CastSpell"),
        MixIngredients = Animator.StringToHash("MixIngredients"),
        Wait = Animator.StringToHash("Wait"),
        Disbelief = Animator.StringToHash("Disbelief");
    #endregion
}
