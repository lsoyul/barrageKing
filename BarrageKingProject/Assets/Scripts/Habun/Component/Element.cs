using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class Element : MonoBehaviour
    {
        [SerializeField]
        private GameStatics.BULLET_TYPE type = GameStatics.BULLET_TYPE.NONE;

        public GameStatics.BULLET_TYPE Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}
