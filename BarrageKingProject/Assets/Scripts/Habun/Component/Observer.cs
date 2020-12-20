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
            if (observer != null) observer.Value = gameObject;
        }

        private void OnEnable()
        {
            if (observerList != null) observerList.Add(gameObject);
        }

        private void OnDisable()
        {
            if (observerList != null) observerList.Remove(gameObject);
        }

    }
}
