using Cysharp.Threading.Tasks;
using PD.UnityEngineExtensions;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Adohi
{
    public enum ViewPoint
    {
        twoDimensional,
        threeDimensional
    }

    public class ViewPointManager : Singleton<ViewPointManager>
    {       
        //events
        public event Action OnViewChangedStartTo2D;
        public event Action OnViewChangedStartTo3D;
        public event Action OnViewChangedMiddleTo2D;
        public event Action OnViewChangedMiddleTo3D;
        public event Action OnViewChangedEndTo2D;
        public event Action OnViewChangedEndTo3D;
        public event Func<bool> ViewPointChangeConditions;

        [Header("View Change Duration")]
        public float firstViewChangeDuration = 1f;
        public float secondViewChangeDuration = 1f;

        [Header("Current State")]
        public bool isViewChanging;
        public ViewPoint currentViewPoint = ViewPoint.twoDimensional;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (ViewPointChangeConditions?.GetInvocationList().Length > 0)
                {
                    if (ViewPointChangeConditions.GetInvocationList().All(f => (bool)f.DynamicInvoke()))
                    {
                        if (!this.isViewChanging)
                        {
                            ChangeViewPointTask();
                        }

                    }
                }
                else
                {
                    if (!this.isViewChanging)
                    {
                        ChangeViewPointTask();
                    }
                }
            }

        }

        public async UniTask ChangeViewPointTask()
        {
            this.isViewChanging = true;

            StartChangeViewPoint();
            await UniTask.Delay((firstViewChangeDuration * 1000).ToInt(), true);

            MiddleChangeViewPoint();
            await UniTask.Delay((secondViewChangeDuration * 1000).ToInt(), true);

            EndChangeViewPoint();
            this.isViewChanging = false;
        }

        public void StartChangeViewPoint()
        {
            switch (currentViewPoint)
            {
                case ViewPoint.twoDimensional:
                    this.currentViewPoint = ViewPoint.threeDimensional;
                    OnViewChangedStartTo3D?.Invoke();
                    break;
                case ViewPoint.threeDimensional:
                    this.currentViewPoint = ViewPoint.twoDimensional;
                    OnViewChangedStartTo2D?.Invoke();
                    break;
            }
        }

        public void MiddleChangeViewPoint()
        {
            switch (currentViewPoint)
            {
                case ViewPoint.twoDimensional:
                    OnViewChangedMiddleTo2D?.Invoke();
                    break;
                case ViewPoint.threeDimensional:
                    OnViewChangedMiddleTo3D?.Invoke();
                    break;
            }
        }

        public void EndChangeViewPoint()
        {
            "End View Changing".Log();
            switch (currentViewPoint)
            {
                case ViewPoint.twoDimensional:
                    OnViewChangedEndTo2D?.Invoke();
                    break;
                case ViewPoint.threeDimensional:
                    OnViewChangedEndTo3D?.Invoke();
                    break;
            }
        }
    }
}