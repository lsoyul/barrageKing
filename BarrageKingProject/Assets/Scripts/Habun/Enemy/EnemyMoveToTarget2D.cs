using PD.UnityEngineExtensions;
using DG.Tweening;
using SGoap;
using Adohi;
using UnityEngine;

namespace Habun
{
    public class EnemyMoveToTarget2D : BasicAction
    {
        public override bool CanAbort() => AgentData.DistanceToTarget <= moveDistance;

        [SerializeField]
        private FovTargetSensor sensor;

        [SerializeField]
        private float moveDistance = 4.0f;
        [SerializeField]
        private float moveTime = 1.0f;
        [SerializeField]
        private float timer = 0.0f;

        public override bool PrePerform()
        {
            if (!sensor.HasTarget)
            {
                return false;
            }

            return base.PrePerform();
        }

        public override EActionStatus Perform()
        {
            if (!sensor.HasTarget)
            {
                return EActionStatus.Failed;
            }

            AgentData.Agent.transform.forward = GetMoveDirection(AgentData.Position, AgentData.Target.position);
            timer += Time.deltaTime;

            if (timer > moveTime)
            {
                if (AgentData.DistanceToTarget > moveDistance)
                {
                    AgentData.Agent.transform.DOMove(GetMovePosition(AgentData.Position, AgentData.Target.position), moveTime);
                }

                timer = 0.0f;
            }

            return CanAbort() ? EActionStatus.Success : EActionStatus.Running;
        }

        private Vector3 GetMoveDirection(Vector3 from, Vector3 to)
        {
            var location = new Location(from.x.RoundToInt(), from.z.RoundToInt());
            var direction = location.OptimalDirectionTo(new Location(to.x.RoundToInt(), to.z.RoundToInt()));

            switch (direction)
            {
                case Direction.Up:
                    return Vector3.forward;
                case Direction.Down:
                    return Vector3.back;
                case Direction.Right:
                    return Vector3.right;
                case Direction.Left:
                    return Vector3.left;
            }

            return Vector3.forward;
        }

        private Vector3 GetMovePosition(Vector3 from, Vector3 to)
        {
            var location = new Location(from.x.RoundToInt(), from.z.RoundToInt());
            var direction = location.OptimalDirectionTo(new Location(to.x.RoundToInt(), to.z.RoundToInt()));

            return (location + direction).ToVector();
        }

    }
}
