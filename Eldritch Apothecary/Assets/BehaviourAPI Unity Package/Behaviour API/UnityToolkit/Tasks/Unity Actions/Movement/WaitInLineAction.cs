using UnityEngine;

namespace BehaviourAPI.UnityToolkit
{
    using System.Collections;
    using System.Collections.Generic;
    using Core;

    /// <summary>
    /// Action that moves the agent waiting in line.
    /// </summary>
    [SelectionGroup("MOVEMENT")]
    public class WaitInLineAction : UnityAction
    {
        public Transform[] positions;
        int positionIndex;

        /// <summary>
        /// Create a new WaitInLineAction
        /// </summary>
        public WaitInLineAction(Transform[] positions)
        {
            this.positions = positions;
        }

        public override void Start()
        {
            context.Movement.SetTarget(positions[0].position);
        }

        public override Status Update()
        {
            if (context.Movement.HasArrived())
            {
                return Status.Success;
            }
            else
            {
                return Status.Running;
            }
        }

        public override void Stop()
        {
            context.Movement.CancelMove();
        }

        public override string ToString() => $"Walk to {context.Movement.GetTarget()}";
    }
}

