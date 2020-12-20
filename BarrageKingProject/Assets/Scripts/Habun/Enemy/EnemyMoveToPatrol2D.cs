using PD.UnityEngineExtensions;
using DG.Tweening;
using SGoap;
using Adohi;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class EnemyMoveToPatrol2D : BasicAction
    {
        [SerializeField]
        private FovTargetSensor sensor;

        [SerializeField]
        private GameObjectValueList patrols;
        [SerializeField]
        private Transform moveTarget;
        [SerializeField]
        private float moveTime = 1.0f;
        [SerializeField]
        private float timer = 0.0f;

        public override EActionStatus Perform()
        {
            moveTarget = moveTarget ? moveTarget : patrols.Get(Random.Range(0, patrols.Count)).transform;
            timer += Time.deltaTime;

            if (timer > moveTime)
            {
                if (Vector3.Distance(moveTarget.position, AgentData.Position) <= Random.Range(0.0f, 1.0f))
                {
                    moveTarget = null;
                }
                else
                {
                    AgentData.Agent.transform.forward = GetMoveDirection(AgentData.Position, moveTarget.position);
                    AgentData.Agent.transform.DOMove(GetMovePosition(AgentData.Position, moveTarget.position), moveTime);
                }

                timer = 0.0f;
            }

            return sensor.HasTarget ? EActionStatus.Success : EActionStatus.Running;
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
