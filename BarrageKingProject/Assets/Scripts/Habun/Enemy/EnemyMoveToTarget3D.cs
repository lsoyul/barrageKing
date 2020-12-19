﻿using SGoap;
using UnityEngine;

namespace Habun
{
    public class EnemyMoveToTarget3D : BasicAction
    {
        public override bool CanAbort() => AgentData.DistanceToTarget <= moveDistance;

        [SerializeField]
        private float moveDistance = 4.0f;
        [SerializeField]
        private float moveSpeed = 2.0f;
        [SerializeField]
        private float lerpDelta = 8.0f;

        public override bool PrePerform()
        {
            if (AgentData.Target == null)
            {
                return false;
            }

            return base.PrePerform();
        }

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
