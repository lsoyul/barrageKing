using System.Collections.Generic;
using SGoap;
using UnityEngine;

namespace Habun
{
    public class EnemyMoveToPatrol2D : BasicAction
    {
        [SerializeField]
        private FovTargetSensor sensor;

        [SerializeField]
        private List<Transform> patrols;
        [SerializeField]
        private Transform moveTarget;

        [SerializeField]
        private float moveSpeed = 5.0f;
        [SerializeField]
        private float lerpDelta = 2.0f;

        public override EActionStatus Perform()
        {
            moveTarget = moveTarget ?? patrols[Random.Range(0, patrols.Count)];
            
            if (Vector3.Distance(moveTarget.position, AgentData.Position) < 1.0f)
            {
                moveTarget = null;
            }
            else
            {
                AgentData.Agent.transform.forward = Vector3.Lerp(AgentData.Agent.transform.forward, (moveTarget.position - AgentData.Position).normalized, lerpDelta * Time.deltaTime);
                AgentData.Position += (moveTarget.position - AgentData.Position).normalized * moveSpeed * Time.deltaTime;
            }

            return sensor.HasTarget ? EActionStatus.Success : EActionStatus.Running;
        }

    }
}
