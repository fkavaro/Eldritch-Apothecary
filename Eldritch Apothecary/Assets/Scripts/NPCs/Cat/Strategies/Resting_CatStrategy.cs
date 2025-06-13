using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// RestStrategy is a strategy for resting the NPC. 
/// It increases energy and changes the animation to idle. 
/// If energy is at max, it returns success. Otherwise, it returns running.
/// </summary>
public class Resting_CatStrategy<TController> : AStrategy<Cat>
{
    Spot restingSpot;

    public Resting_CatStrategy(Cat controller)
    : base(controller) { }

    public override Node<Cat>.Status Update()
    {
        if (restingSpot == null)
        {
            restingSpot = _controller.CloserRestingSpot();

            if (restingSpot == null)
            {
                if (_controller.debugMode) Debug.Log(_controller.name + " has nowhere to rest");
            }
            else
                _controller.SetDestinationSpot(restingSpot);
        }
        else if (_controller.HasArrived(restingSpot))
        {
            _controller.ChangeAnimationTo(_controller.idleAnim);

            _controller.IncreaseEnergy(Time.deltaTime * 2);

            if (_controller.IsEnergyAtMax())
            {
                if (_controller.debugMode) Debug.Log(_controller.name + " is fully rested");
                _controller.isStopped = false;
                return Node<Cat>.Status.Success;
            }
            else
            {
                _controller.isStopped = true;
                return Node<Cat>.Status.Running;
            }
        }
        return Node<Cat>.Status.Running;
    }
}
