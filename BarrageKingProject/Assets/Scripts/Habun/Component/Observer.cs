using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class Observer : MonoBehaviour
    {
        [SerializeField]
        private GameObjectReference observer;

        // MONOBEHAVIOUR METHODS: ----------------------------------------------------

        private void Start()
        {
            observer.Value = gameObject;
        }

    }
}
