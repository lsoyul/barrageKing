using SGoap;
using UnityEngine;

namespace Habun
{
    public class EnemyMoveToTarget2D : BasicAction
    {
        public override bool CanAbort() => AgentData.DistanceToTarget <= moveDistance;

        [SerializeField]
        private float moveDistance = 5.0f;
        [SerializeField]
        private float moveSpeed = 5.0f;
        [SerializeField]
        private float lerpDelta = 2.0f;

        public override EActionStatus Perform()
        {
            AgentData.Agent.transform.forward = Vector3.Lerp(AgentData.Agent.transform.forward, AgentData.DirectionToTarget, lerpDelta * Time.deltaTime);

            if (AgentData.DistanceToTarget > moveDistance)
            {
                AgentData.Position += AgentData.DirectionToTarget * moveSpeed * Time.deltaTime;
            }

            return CanAbort() ? EActionStatus.Success : EActionStatus.Running;
        }

    }
}
