using System.Collections.Generic;
using System.Linq;
using SGoap;
using UnityEngine;

namespace Habun
{
    public class EnemyAttack2D : BasicAction
    {
        public override float CooldownTime => 2;
        public override float StaggerTime => 1.5f;

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
            return !Cooldown.Active ? EActionStatus.Success : EActionStatus.Failed;
        }

        public override bool PostPerform()
        {
            foreach (var bullet in bullets)
            {
                //bullet.StartShooting();
            }

            return base.PrePerform();
        }

        public new void Bind(AgentBasicData data)
        {
            base.Bind(data);

            bullets = AgentData.Agent.GetComponentsInChildren<BulletShooter>().ToList();
        }

    }
}
