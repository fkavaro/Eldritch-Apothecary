using UnityEngine;

/// <summary>
/// Abstract class that determines the humanoid properties of the NPCs and its animations.
/// </summary>
public abstract class AHumanoid<TController> : ANPC<TController>
where TController : ABehaviourController<TController>
{
    [Header("Humanoid Properties")]
    [Tooltip("Triggering distance to cat"), Range(0, 4)]
    public int minDistanceToCat = 1;
    [Tooltip("Probability of being scared"), Range(0, 10)]
    public int fear = 0;

    [Tooltip("Maximum number of scares supported")]
    public int maxScares = 100;

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

    protected int _scaresCount = 0;

    /// <returns> True if the cat is too close and the client is scared, false otherwise.</returns>
    public bool CatIsTooClose()
    {
        // Cat is close and client is scared
        if (Vector3.Distance(transform.position, ApothecaryManager.Instance.cat.transform.position) < minDistanceToCat &&
            Random.Range(0, 10) < fear)
        {
            _scaresCount++;
            return true;
        }
        // Cat is too far or client is not scared
        else return false;
    }

    /// <returns>If client has been scared enough times</returns>
    public bool HasReachedMaxScares()
    {
        return _scaresCount >= maxScares;
    }
}
