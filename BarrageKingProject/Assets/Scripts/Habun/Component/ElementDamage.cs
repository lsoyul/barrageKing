using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class ElementDamage : Damage
    {
        public override void Hit(Collider collider)
        {
            var wall = GetComponentInParent<Wall>();
            if ((mask.value & (1 << collider.gameObject.layer)) > 0)
            {
                var element = collider.GetComponent<Element>();
                if (!element || element.Type != wall.Type) return;

                var health = collider.GetComponent<Health>();
                if (health) health.Damage(damage.Value);

                onHit.Invoke(collider);
            }
        }

    }
}
