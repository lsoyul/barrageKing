using CloudFine;
using SGoap;
using UnityEngine;

namespace Habun
{
    public class EnemyMovement : BasicAction
    {
        [SerializeField]
        private SteeringAgent steering;

        [SerializeField]
        private FovTargetSensor fov;

        public override EActionStatus Perform()
        {
            if (steering) steering.LockPosition(fov.HasTarget);

            return fov.HasTarget ? EActionStatus.Success : EActionStatus.Running;
        }

    }
}
