using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class Damage : MonoBehaviour
    {
        [SerializeField]
        protected FloatReference damage;
        [SerializeField]
        protected LayerMask mask;

        [SerializeField, Space]
        protected ColliderUnityEvent onHit;

        // PUBLIC METHODS: ----------------------------------------------------

        public virtual void Hit(Collider collider)
        {
            if ((mask.value & (1 << collider.gameObject.layer)) > 0)
            {
                var health = collider.GetComponent<Health>();
                if (health) health.Damage(damage.Value);

                onHit.Invoke(collider);
            }
        }

        // CALLBACK METHODS: ----------------------------------------------------

        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (collider == null)
            {
                return;
            }

            Hit(collider);
        }

    }
}
