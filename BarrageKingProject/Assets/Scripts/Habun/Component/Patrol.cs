using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField]
        private GameObjectValueList patrols;

        // MONOBEHAVIOUR METHODS: ----------------------------------------------------

        private void OnEnable()
        {
            patrols.Add(gameObject);
        }

        private void OnDisable()
        {
            patrols.Remove(gameObject);
        }

    }
}
