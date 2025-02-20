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
        private int positionIndex;

        /// <summary>
        /// Create a new WaitInLineAction
        /// </summary>
        public WaitInLineAction()
        {
            positionIndex = 0;
        }

        public override void Start()
        {
            // Move to the last position in line (behind the last client)
            //context.Movement.SetTarget(ApothecaryManager.Instance.nextClientPosition.position);
        }

        public override Status Update()
        {
            if (context.Movement.HasArrived())
            {
                // Is first in line
                if (positionIndex == 0)
                {
                    return Status.Success;
                }
                else // Is not first in line
                {
                    // Move to the next position
                    positionIndex--;
                    context.Movement.SetTarget(positions[positionIndex].position);
                    return Status.Running;
                }
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

