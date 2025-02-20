using UnityEngine;

/// <summary>
/// Defines a common class for all animation controlles.
/// Handles animation transitions.
/// </summary>
[RequireComponent(typeof(Animator))]
public abstract class AAnimationController : ABehaviourController
{
    protected Animator animator;
    protected int currentAnimation;

    // Specific animations must be defined in derived classes

    protected override void OnAwake()
    {
        animator = GetComponent<Animator>();

        AwakeFrame();
    }

    public virtual void AwakeFrame() { }

    protected override void OnUpdate()
    {

        CheckAnimation();
    }

    /// <summary>
    /// Checks animation conditions.
    /// </summary>
    public abstract void CheckAnimation();

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
}
