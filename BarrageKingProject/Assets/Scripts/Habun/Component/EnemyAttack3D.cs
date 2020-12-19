using CloudFine;
using SGoap;
using UnityEngine;

namespace Habun
{
    public class EnemyAttack3D : BasicAction
    {
        [SerializeField]
        private SteeringAgent steering;

        [SerializeField]
        private FovTargetSensor fov;

        [SerializeField]
        private float lerpDelta = 5.0f;

        [SerializeField]
        private PoolObject bullet;

        public override EActionStatus Perform()
        {
            AgentData.Agent.transform.forward = Vector3.Lerp(AgentData.Agent.transform.forward, -AgentData.DirectionToTarget, lerpDelta * Time.deltaTime);

            if (steering)
            {
                steering.LockPosition(true);
            }

            //var instance = PoolManager.Instance.Pick(bullet);
            //instance.transform.SetPositionAndRotation(transform.position, transform.rotation);

            Debug.Log("Attack!!");

            return fov.HasTarget ? EActionStatus.Success : EActionStatus.Failed;
        }

    }
}
