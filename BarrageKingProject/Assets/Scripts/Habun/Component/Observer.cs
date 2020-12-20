using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class Observer : MonoBehaviour
    {
        [SerializeField]
        private GameObjectReference observer;

        [SerializeField]
        private GameObjectValueList observerList;

        // MONOBEHAVIOUR METHODS: ----------------------------------------------------

        private void Start()
        {
            observer.Value = gameObject;
        }

        private void OnEnable()
        {
            observerList.Add(gameObject);
        }

        private void OnDisable()
        {
            observerList.Remove(gameObject);
        }

    }
}
