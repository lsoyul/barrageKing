using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class BulletShooter : MonoBehaviour
{
    bool firing = false;
    float timer = 0f;

    int curPatternIndex = 0;

    public bool autoStart = true;
    public List<pattern> patternList;
    

    [Serializable]
    public struct pattern
    {
        public float delay;
        public float speed;
        public float acceleration;
        public GameStatics.BULLET_TYPE bulletType;
    }

    [ContextMenu("StartShooting")]
    public void StartShooting()
    {
        if (patternList.Count <= 0) return;

        firing = true;

        timer = 0;
        curPatternIndex = 0;
    }

    public void StopShooting()
    {
        firing = true;

        timer = 0;
        curPatternIndex = 0;
    }

    private void Start()
    {
        if (autoStart) firing = true;
        else firing = false;
    }


    public void Update()
    {
        if (firing)
        {
            timer += Time.deltaTime;

            if (timer > patternList[curPatternIndex].delay)
            {
                if (patternList[curPatternIndex].bulletType != GameStatics.BULLET_TYPE.NONE)
                    BulletManager.Instance().FireBullet(patternList[curPatternIndex].bulletType, this.transform.position, 5f, patternList[curPatternIndex].speed, patternList[curPatternIndex].acceleration, this.transform.forward);

                if (++curPatternIndex >= patternList.Count) curPatternIndex = 0;

                timer = 0;
            }
        }
    }
}
