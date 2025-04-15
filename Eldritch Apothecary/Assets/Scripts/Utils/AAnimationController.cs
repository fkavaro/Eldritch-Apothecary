using UnityEngine;
using System.Collections;

/// <summary>
/// Defines a common class for all animation controllers.
/// Handles animation transitions.
/// </summary>
[RequireComponent(typeof(Animator))]
public abstract class AAnimationController<TController> : ABehaviourController<TController>
where TController : ABehaviourController<TController>
{
    protected Animator animator;
    protected int currentAnimation;

    #region COMMON ANIMATIONS
    readonly public int idleAnim = Animator.StringToHash("Idle"),
        walkAnim = Animator.StringToHash("Walk");
    #endregion
    // Specific animations must be defined in derived classes

    #region INHERITED METHODS
    protected override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }
    #endregion

    #region PUBLIC METHODS
    /// <summary>
    /// Crossfade to new animation.
    /// </summary>
    public virtual void ChangeAnimationTo(int newAnimation, float duration = 0.2f)
    {
        // Not same as current
        if (currentAnimation != newAnimation)
        {
            currentAnimation = newAnimation;

            // Interpolate transition to new animation
            animator.CrossFade(newAnimation, duration);
        }
    }
    #endregion
}
