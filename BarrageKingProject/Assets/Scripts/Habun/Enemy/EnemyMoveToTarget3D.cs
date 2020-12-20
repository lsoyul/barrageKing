using SGoap;
using UnityEngine;

namespace Habun
{
    public class EnemyMoveToTarget3D : BasicAction
    {
        public override bool CanAbort() => AgentData.DistanceToTarget <= moveDistance;

        [SerializeField]
        private FovTargetSensor sensor;

        [SerializeField]
        private float moveDistance = 4.0f;
        [SerializeField]
        private float moveSpeed = 2.0f;
        [SerializeField]
        private float lerpDelta = 8.0f;

        public override EActionStatus Perform()
        {
            if (!sensor.HasTarget)
            {
                return EActionStatus.Failed;
            }

            AgentData.Agent.transform.forward = Vector3.Lerp(AgentData.Agent.transform.forward, AgentData.DirectionToTarget, lerpDelta * Time.deltaTime);

            if (AgentData.DistanceToTarget > moveDistance)
            {
                AgentData.Position += AgentData.Agent.transform.forward * moveSpeed * Time.deltaTime;
            }

            return CanAbort() ? EActionStatus.Success : EActionStatus.Running;
        }

    }
}
