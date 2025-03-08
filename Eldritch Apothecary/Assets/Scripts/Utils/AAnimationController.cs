using UnityEngine;
using System.Collections;

/// <summary>
/// Defines a common class for all animation controllers.
/// Handles animation transitions.
/// </summary>
[RequireComponent(typeof(Animator))]
public abstract class AAnimationController : ABehaviourController
{
    protected Animator animator;
    protected int currentAnimation;

    #region COMMON ANIMATIONS
    readonly public int Idle = Animator.StringToHash("Idle"),
        Walk = Animator.StringToHash("Walk");
    #endregion

    // Specific animations must be defined in derived classes

    protected override void OnAwake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnUpdate()
    {
        CheckAnimation();
    }

    /// <summary>
    /// Checks animation conditions.
    /// </summary>
    public virtual void CheckAnimation() { }

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

    /// <summary>
    /// Waits for a specified number of seconds.
    /// </summary>
    public IEnumerator Wait(float seconds)
    {

        yield return new WaitForSeconds(seconds);
    }
}
