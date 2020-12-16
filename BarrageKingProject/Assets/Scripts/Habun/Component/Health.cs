using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private FloatReference hp;
        [SerializeField]
        private FloatReference maxHp;

        [SerializeField, Space]
        private GameObjectUnityEvent onDeath;

        // PROPERTIES: ---------------------------------------------------------

        public float HP
        {
            get { return hp.Value; }
            set { hp.Value = value; }
        }

        // PUBLIC METHODS: ----------------------------------------------------

        public void Damage(float damage)
        {
            if (hp.Value > 0.0f)
            {
                hp.Value = Mathf.Min(0.0f, hp.Value - damage);

                if (hp.Value <= 0.0f)
                {
                    onDeath.Invoke(gameObject);
                }
            }
        }

        // MONOBEHAVIOUR METHODS: ----------------------------------------------------

        private void Start()
        {
            hp.Value = maxHp.Value;
        }

    }
}
