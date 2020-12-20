using Cysharp.Threading.Tasks;
using MoreLinq;
using PD.UnityEngineExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Adohi
{
    public class BoxController : MonoBehaviour
    {
        Character character;
        private bool isPushAvailable;
        private bool isConstructAvailable;
        public bool isConstructing = false;

        [Header("Box Prefab")]
        public Box boxPrefab;

        [Header("ContructSetting")]
        public KeyCode constructKey = KeyCode.Mouse0;
        public int maxBoxCount = 5;
        public float jumpHeight = 2f;
        public float consturctDelay = 0.5f;
        public float pushPower;
        public float pushDuration;

        [Header("LockOn")]
        public Box lockOnBox;
        public float minLockOnDistance = 2f;
        
        [Header("PushSetting")]
        public KeyCode pushstructKey = KeyCode.Mouse1;



        public int CurrentBoxCount { get => MapManager.Instance.boxes.Count; }

        private void Start()
        {
            ViewPointManager.Instance.ViewPointChangeConditions += () => !isConstructing;
        }

        private void Update()
        {
            if (PushAvailable())
            {
                switch (ViewPointManager.Instance.currentViewPoint)
                {
                    case ViewPoint.twoDimensional:
                        LockOn2D(character.currentLocation, character.alignDirection);
                        break;
                    case ViewPoint.threeDimensional:
                        LockOn3D(character.transform.position);
                        break;
                }

                if (Input.GetKeyDown(constructKey))
                {
                    if (lockOnBox != null)
                    {
                        switch (ViewPointManager.Instance.currentViewPoint)
                        {
                            case ViewPoint.twoDimensional:
                                Push2D(lockOnBox, this.character.currentLocation.OptimalDirectionTo(lockOnBox.location));
                                break;
                            case ViewPoint.threeDimensional:
                                Push3D(lockOnBox, character.forwardVector);
                                break;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(constructKey))
            {
                if (ConstuctAvailable())
                {
                    switch (ViewPointManager.Instance.currentViewPoint)
                    {
                        case ViewPoint.twoDimensional:
                            Construct2D(character.currentLocation, character.alignDirection, consturctDelay);
                            break;
                        case ViewPoint.threeDimensional:
                            Construct3D(character.transform.position, this.consturctDelay);
                            break;
                    }
                }
            }
        }

        public async UniTask Construct2D(Location characterLocation, Direction direction, float duration)
        {
            isConstructing = true;
            await UniTask.Delay((duration * 1000).ToInt());
            var targetLocation = characterLocation + direction;
            if (MapManager.Instance.map2D[targetLocation.X, targetLocation.Y] == 1)
            {
                var constructedBox = Instantiate(boxPrefab);
                constructedBox.Construct2D(targetLocation);
                MapManager.Instance.AddBox(boxPrefab);
            }
            isConstructing = false;
        }

        public async UniTask Construct3D(Vector3 characterPosition, float duration)
        {
            isConstructing = true;
            await Jump(this.jumpHeight, duration);
            var point = RaycastUtil.RaycastPoint(this.transform.position, Vector3.down, LayerMask.NameToLayer("Ground"), out var isHit);
            if (isHit)
            {
                var targetPosition = point + 0.5f * Vector3.up;
                var constructedBox = Instantiate(boxPrefab);
                constructedBox.Construct3D(targetPosition);
                MapManager.Instance.AddBox(boxPrefab);
            }
            isConstructing = false;
        }

        public async UniTask Jump(float height, float duration)
        {
            await UniTask.Delay((duration * 1000).ToInt());
            this.transform.position += Vector3.up * height;
        }

        void LockOn2D(Location characterLocation, Direction direction)
        {
            var targetLocation = characterLocation + direction;
            var box = MapManager.Instance.GetBox(targetLocation);
            if (box != null && !box.IsPushing)
            {
                this.lockOnBox = box;
            }
            else if (box == null)
            {
                this.lockOnBox = null;
            }
        }


        void Push2D(Box box, Direction direction)
        {
            //box.Push2DTask(this.pushPower.ToInt(), this.pushDuration, this.character.currentLocation.OptimalDirectionTo(box.location));
            box.Push2DTask(this.pushPower.ToInt(), this.pushDuration, direction);
        }


        void LockOn3D(Vector3 position)
        {
            var box = MapManager.Instance.boxes.MinBy(b => Vector3.Distance(position, b.transform.position)).FirstOrDefault();
            if (box != null && !box.IsPushing)
            {
                this.lockOnBox = box;
            }
            else if (box == null)
            {
                this.lockOnBox = null;
            }
        }

        void Push3D(Box box, Vector3 pushDirectionVector)
        {
            box.Push3DTask(pushPower, pushDuration, pushDirectionVector);
        }

        public bool ConstuctAvailable()
        {
            switch (ViewPointManager.Instance.currentViewPoint)
            {
                case ViewPoint.twoDimensional:
                    return !ViewPointManager.Instance.isViewChanging && !character.characterController.isMoving2D && !this.isConstructing;
                case ViewPoint.threeDimensional:
                    return !ViewPointManager.Instance.isViewChanging && !character.characterController.isGround3D && !this.isConstructing;
            }
            return false;
        }

        public bool PushAvailable()
        {
            switch (ViewPointManager.Instance.currentViewPoint)
            {
                case ViewPoint.twoDimensional:
                    return !ViewPointManager.Instance.isViewChanging && !character.characterController.isMoving2D && !this.isConstructing;
                case ViewPoint.threeDimensional:
                    return !ViewPointManager.Instance.isViewChanging && !character.characterController.isGround3D && !this.isConstructing;
            }
            return false;
        }


    }
}