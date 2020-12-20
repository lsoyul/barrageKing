using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Adohi
{
    public class CharacterStatus : MonoBehaviour
    {
        public float maxHealth;
        public FloatReactiveProperty currentHealth;

        private void Awake()
        {
            currentHealth = new FloatReactiveProperty(maxHealth);
        }
    }

}
