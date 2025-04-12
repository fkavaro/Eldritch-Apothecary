using UnityEngine;

public abstract class AHumanoid : ANPC
{
    [Header("Humanoid Properties")]
    [Tooltip("Triggering distance to cat"), Range(0f, 4f)]
    public float minDistanceToCat = 3f;

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
