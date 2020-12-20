using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class Damage : MonoBehaviour
    {
        [SerializeField]
        private FloatReference damage;
        [SerializeField]
        private LayerMask mask;

        [SerializeField, Space]
        private ColliderUnityEvent onHit;

        // PUBLIC METHODS: ----------------------------------------------------

        public void Hit(Collider collider)
        {
            if ((mask.value & (1 << collider.gameObject.layer)) > 0)
            {
                var health = collider.GetComponent<Health>();
                if (health) health.Damage(damage.Value);

                onHit.Invoke(collider);
            }
        }

        // CALLBACK METHODS: ----------------------------------------------------

        private void OnTriggerEnter(Collider collider)
        {
            if (collider == null)
            {
                return;
            }

            Hit(collider);
        }

    }
}
