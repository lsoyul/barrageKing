using System.Collections;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class PoolObject : MonoBehaviour
    {
        [SerializeField]
        private IntReference initCount;
        [SerializeField]
        private FloatReference duration;

        // VARIABLES: ---------------------------------------------------------

        private IEnumerator coroutine;

        // PROPERTIES: ---------------------------------------------------------

        public int InitCount
        {
            get { return initCount.Value; }
            set { initCount.Value = value; }
        }

        // PRIVATE METHODS: ----------------------------------------------------

        private IEnumerator SetDisable()
        {
            if (duration < 0.0f)
            {
                yield break;
            }

            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
        }

        // MONOBEHAVIOUR METHODS: ----------------------------------------------------

        private void OnEnable()
        {
            coroutine = SetDisable();
            StartCoroutine(coroutine);
        }

        private void OnDisable()
        {
            CancelInvoke();
            StopCoroutine(coroutine);
        }

    }
}
