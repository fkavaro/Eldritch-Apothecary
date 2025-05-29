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
public class RestStrategy<TController> : AStrategy<TController>
where TController : ANPC<TController>
{
    public RestStrategy(TController controller)
    : base(controller) { }

    public override Node<TController>.Status Update()
    {
        _controller.ChangeAnimationTo(_controller.idleAnim);

        _controller.IncreaseEnergy(Time.deltaTime);

        if (_controller.IsEnergyAtMax())
        {
            if (_controller.debugMode) Debug.Log(_controller.name + " is fully rested");
            return Node<TController>.Status.Success;
        }
        else
            return Node<TController>.Status.Running;
    }
}
