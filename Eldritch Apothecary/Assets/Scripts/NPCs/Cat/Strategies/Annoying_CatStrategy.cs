using Unity.Mathematics;
using UnityEngine;

public class Annoying_CatStrategy : AStrategy<Cat>
{
    readonly Transform _positionWhereAnnoy;
    readonly bool _isAnnoyinAlchemist;
    bool _hasAnimationFinished = false,
        _hasAnimationStarted = false;

    public Annoying_CatStrategy(Cat controller, Transform positionWhereAnnoy, bool isAnnoyinAlchemist) : base(controller)
    {
        _positionWhereAnnoy = positionWhereAnnoy;
        _isAnnoyinAlchemist = isAnnoyinAlchemist;
    }

    public override Node<Cat>.Status Update()
    {
        // Set position where annoy as destination if it's not already
        if (_controller.GetDestinationPos() != _positionWhereAnnoy.position)
            _controller.SetDestination(_positionWhereAnnoy.position);

        // Keep annoying for some time if destination arrived
        if (!_hasAnimationStarted && _controller.HasArrivedAtDestination())
        {
            _hasAnimationStarted = true;
            _controller.PlayAnimationRandomTime(_controller.idleAnim, "Annoying", AnimationFinished);
            return Node<Cat>.Status.Running;
        }
        else
        {
            if (_hasAnimationFinished)
            {
                _hasAnimationStarted = false;
                _hasAnimationFinished = false;
                return Node<Cat>.Status.Success;
            }
            else
                return Node<Cat>.Status.Running;
        }
    }

    void AnimationFinished()
    {
        if (_isAnnoyinAlchemist)
            _controller.lastTimeAlchemistWasAnnoyed = Time.time;
        else
            _controller.lastTimeSorcererWasAnnoyed = Time.time;
        Cat.RaiseSorcererNoLongerAnnoyed();

        if (_controller.debugMode) Debug.Log(_controller.name + " finished annoying");
        _hasAnimationFinished = true;
    }
}