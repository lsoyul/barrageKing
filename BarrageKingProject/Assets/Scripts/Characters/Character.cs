using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Adohi
{
    public class Character : MonoBehaviour
    {
        private float startMouseX;
        private float startMouseY;
        private Vector3 currentRotation;

        public GameObject model2D;
        public GameObject model3D;
        public Animator animator2D;
        public Animator animator3D;

        public float sensitivity = 10f;
        public float minYAngle;
        public float maxYAngle;
        public Location currentLocation;

        public Vector3 forwardVector;
        public Vector3 XZForward { get => new Vector3(forwardVector.x, 0f, forwardVector.z).normalized; }

        [Header("2D Value")]
        public Direction alignDirection = Direction.Down;


        private void Start()
        {
            ViewPointManager.Instance.OnViewChangedEndTo3D += () => RotateStart();
            ViewPointManager.Instance.OnViewChangedMiddleTo2D += () => ChangeCharacterModel(ViewPointManager.Instance.currentViewPoint);
            ViewPointManager.Instance.OnViewChangedMiddleTo3D += () => ChangeCharacterModel(ViewPointManager.Instance.currentViewPoint);
        }
        // Update is called once per frame
        void Update()
        {
            if (ViewPointManager.Instance.currentViewPoint == ViewPoint.threeDimensional)
            {
                currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
                currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
                currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
                currentRotation.y = Mathf.Clamp(currentRotation.y, minYAngle, maxYAngle);
                this.forwardVector = Quaternion.Euler(currentRotation.y, currentRotation.x, 0) * Vector3.forward;
                //this.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
            }

        }

        public void RotateStart()
        {
            currentRotation = Vector3.zero;
        }

        public void ChangeCharacterModel(ViewPoint viewPoint)
        {
            switch (viewPoint)
            {
                case ViewPoint.twoDimensional:
                    this.model2D.SetActive(true);
                    this.model3D.SetActive(false);
                    break;
                case ViewPoint.threeDimensional:
                    this.model2D.SetActive(false);
                    this.model3D.SetActive(true);
                    break;
            }
        }

    }
}