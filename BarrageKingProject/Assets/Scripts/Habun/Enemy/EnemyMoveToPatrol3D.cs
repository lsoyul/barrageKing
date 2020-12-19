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
        private Transform moveTransform;

        [SerializeField]
        private float moveSpeed = 5.0f;
        [SerializeField]
        private float lerpDelta = 2.0f;

        public override EActionStatus Perform()
        {
            moveTransform = moveTransform ?? patrols.Get(Random.Range(0, patrols.Count)).transform;

            if (Vector3.Distance(moveTransform.position, AgentData.Position) < 1.0f)
            {
                moveTransform = null;
            }
            else
            {
                AgentData.Agent.transform.forward = Vector3.Lerp(AgentData.Agent.transform.forward, (moveTransform.position - AgentData.Position).normalized, lerpDelta * Time.deltaTime);
                AgentData.Position += (moveTransform.position - AgentData.Position).normalized * moveSpeed * Time.deltaTime;
            }

            return sensor.HasTarget ? EActionStatus.Success : EActionStatus.Running;
        }

    }
}
