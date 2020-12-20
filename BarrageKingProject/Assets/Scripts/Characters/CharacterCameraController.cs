using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{
    public class CharacterCameraController : MonoBehaviour
    {
        private Camera camera;
        private float currentDistance3D;
        public Character target;



        [Header("2DSetting")]
        public float distance2D = 10f;
        public float followSpeed2D = 1f;

        [Header("3DSetting")]
        public float distance3D = 10f;
        public float followSpeed3D = 1f;
        public Vector3 followOffset;

        private void Awake()
        {
            this.camera = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                switch (ViewPointManager.Instance.currentViewPoint)
                {
                    case ViewPoint.twoDimensional:
                        Follow2D();
                        break;
                    case ViewPoint.threeDimensional:
                        Follow3D();
                        break;
                }
            }

        }

        public void Follow2D()
        {
            this.camera.orthographic = true;
            this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position + Vector3.up * distance2D, Time.deltaTime * followSpeed2D);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(Vector3.down, Vector3.forward), Time.deltaTime * followSpeed2D);

        }

        public void Follow3D()
        {
            this.camera.orthographic = false;
            AvoidObstacle();
            this.transform.position = Vector3.Lerp(this.transform.position, (target.transform.position + followOffset) - target.forwardVector * currentDistance3D, Time.deltaTime * followSpeed3D);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(target.forwardVector, Vector3.up), Time.deltaTime * followSpeed3D);
        }


        public void AvoidObstacle()
        {
            if (IsGroundIntercept(out float hitDistance))
            {
                this.currentDistance3D = hitDistance;
            }
            else
            {
                this.currentDistance3D = Mathf.Lerp(currentDistance3D, distance3D, Time.deltaTime * followSpeed3D);
            }
        }

        private bool IsGroundIntercept(out float distance)
        {
            var ret = Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hitinfo, Vector3.Distance(this.transform.position, target.transform.position), 1 << LayerMask.NameToLayer("Ground"));
            distance = Vector3.Distance(hitinfo.point, target.transform.position);
            return ret;
        }
    }
}