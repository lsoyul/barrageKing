using SGoap;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class EnemyMoveToPatrol3D : BasicAction
    {
        [SerializeField]
        private FovTargetSensor sensor;
        [SerializeField]
        private GameObjectValueList patrols;

        [SerializeField]
        private Transform moveTarget;
        [SerializeField]
        private float moveSpeed = 4.0f;
        [SerializeField]
        private float lerpDelta = 8.0f;

        public override bool PrePerform()
        {
            if (patrols.Count == 0)
            {
                return false;
            }

            return base.PrePerform();
        }

        public override EActionStatus Perform()
        {
            moveTarget = moveTarget ?? patrols.Get(Random.Range(0, patrols.Count)).transform;

            if (Vector3.Distance(moveTarget.position, AgentData.Position) < 0.5f)
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
