using UnityEngine;

public abstract class AHumanoid : ANPC
{
    #region HUMANOID ANIMATIONS
    readonly public int talkAnim = Animator.StringToHash("Talk"),
        pickUpAnim = Animator.StringToHash("PickUp"),
        complainAnim = Animator.StringToHash("Complain"),
        stunnedAnim = Animator.StringToHash("Stunned"),
        sitDownAnim = Animator.StringToHash("SitDown"),
        standUpAnim = Animator.StringToHash("StandUp"),
        argueAnim = Animator.StringToHash("Argue"),
        yellAnim = Animator.StringToHash("Yell"),
        castSpellAnim = Animator.StringToHash("CastSpell"),
        mixIngredientsAnim = Animator.StringToHash("MixIngredients"),
        waitAnim = Animator.StringToHash("Wait"),
        disbeliefAnim = Animator.StringToHash("Disbelief");
    #endregion
}
