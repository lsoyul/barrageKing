using PD.UnityEngineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Adohi
{
    public class Character : MonoBehaviour
    {
        private Vector3 currentRotation;
        private Rigidbody rb;

        [Header("Components")]
        public CustomCharacterController characterController;
        public BoxController boxController;
        public CharacterStatus status;

        [Header("Models")]
        public GameObject model2D;
        public GameObject model3D;
        public Animator animator2D;
        public Animator animator3D;

        [Header("ChracterForwardSetting")]
        public float sensitivity = 10f;
        public float minYAngle = -20f;
        public float maxYAngle = 80f;

        [Header("CharacterStatus")]
        public Location currentLocation;
        public Vector3 forwardVector;
        public Vector3 XZForward { get => new Vector3(forwardVector.x, 0f, forwardVector.z).normalized; }

        [Header("2D Value")]
        public Direction alignDirection = Direction.Down;


        private void Awake()
        {
            this.characterController = GetComponent<CustomCharacterController>();
            this.boxController = GetComponent<BoxController>();
            this.status = GetComponent<CharacterStatus>();
            this.rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            ViewPointManager.Instance.OnViewChangedEndTo3D += () => Rotate3DStart();
            ViewPointManager.Instance.OnViewChangedMiddleTo2D += () => ChangeCharacterModel(ViewPointManager.Instance.currentViewPoint);
            ViewPointManager.Instance.OnViewChangedMiddleTo3D += () => ChangeCharacterModel(ViewPointManager.Instance.currentViewPoint);
        }

        // Update is called once per frame
        void Update()
        {
            if (ViewPointManager.Instance.currentViewPoint == ViewPoint.threeDimensional)
            {
                Rotate3D();
            }

        }

        public void ConnectCamera()
        {
            Camera.main.GetComponent<CharacterCameraController>().target = this;
        }
        public void Rotate3D()
        {
            currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360f);
            currentRotation.y = Mathf.Clamp(currentRotation.y, minYAngle, maxYAngle);
            this.forwardVector = Quaternion.Euler(currentRotation.y, currentRotation.x, 0) * Vector3.forward;
        }

        public void Rotate3DStart()
        {
            currentRotation = alignDirection.ToRotation().eulerAngles;
        }

        public void SyncPosition()
        {
            this.transform.position = this.currentLocation.ToVector();
        }

        public void SyncLocation()
        {
            this.currentLocation = new Location(this.transform.position.x.RoundToInt(), this.transform.position.y.RoundToInt());
        }

        public void ChangeCharacterModel(ViewPoint viewPoint)
        {
            switch (viewPoint)
            {
                case ViewPoint.twoDimensional:
                    this.model2D.SetActive(true);
                    this.model3D.SetActive(false);
                    //rb.fre
                    break;
                case ViewPoint.threeDimensional:
                    this.model2D.SetActive(false);
                    this.model3D.SetActive(true);
                    break;
            }
        }

    }
}