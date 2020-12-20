using System.Collections.Generic;
using System.Linq;
using SGoap;
using UnityEngine;

namespace Habun
{
    public class EnemyAttack2D : BasicAction
    {
        public override float CooldownTime => 1.0f;
        public override float StaggerTime => 0.5f;

        [SerializeField]
        private List<BulletShooter> bullets;

        public override bool PrePerform()
        {
            foreach (var bullet in bullets)
            {
                bullet.StartShooting();
            }

            return base.PrePerform();
        }

        public override EActionStatus Perform()
        {
            if (AgentData.Target)
            {
                AgentData.Agent.transform.forward = AgentData.DirectionToTarget;
            }

            return base.Perform();
        }

        public override bool PostPerform()
        {
            foreach (var bullet in bullets)
            {
                bullet.StopShooting();
            }

            return base.PostPerform();
        }

        public override void Bind(AgentBasicData data)
        {
            base.Bind(data);

            bullets = AgentData.Agent.GetComponentsInChildren<BulletShooter>().ToList();
        }

    }
}
