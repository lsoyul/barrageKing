using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{
    public static class RaycastUtil
    {

        public static Vector3 RaycastPoint(Vector3 startPosition, Vector3 direction, int layer, out bool isHit)
        {
            if (Physics.Raycast(startPosition, direction, out var hitinfo, 1 << LayerMask.NameToLayer("Ground")))
            {
                isHit = true;
                return hitinfo.point;
            }
            else
            {
                isHit = false;
                return Vector3.zero;
            }
        }

    }

}
