using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Rendering;

public class BulletBase : MonoBehaviour
{
    public GameStatics.BULLET_TYPE bulletType = GameStatics.BULLET_TYPE.NORMAL1;
    public float speed = 1f;
    public float acceleration = 0f;
    public float radius = 5f;                   // Collider radius

    public float maxLivingDuration = 1000f;          
    public float maxLivingDistance = 500f;

    private float curTimer = 0;
    private Vector3 curVelocity = Vector3.zero;

    [SerializeField] private bool isFire = false;
    private Vector3 initPosition = Vector3.zero;

    public Action<BulletBase> onDestroy;

    public Vector3 GetDirection()
    {
        return this.transform.forward;
    }


    public virtual void Fire(GameStatics.BULLET_TYPE bulletType, float radius, float speed, float acceleration, Vector3 direction)
    {
        this.bulletType = bulletType;
        this.radius = radius;
        this.speed = speed;
        this.acceleration = acceleration;

        Vector3 lookAtPoint = this.transform.position;
        lookAtPoint += direction.normalized;

        this.transform.LookAt(lookAtPoint);

        this.curVelocity = this.transform.forward * speed;
        this.curTimer = 0;
        this.initPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        this.isFire = true;
    }

    protected virtual void Update()
    {
        if (isFire)
        {
            if (Mathf.Approximately(acceleration, 0f) == false) speed += acceleration;
            
            curVelocity = this.transform.forward * speed;

            this.transform.position += curVelocity * Time.deltaTime;

            curTimer += Time.deltaTime;

            if (curTimer > maxLivingDuration
                || Vector3.Distance(this.gameObject.transform.position, this.initPosition) > maxLivingDistance)
            {
                // if (living time over) or (far away from max distance) than return to datapool
                if (onDestroy != null) onDestroy(this);
            }
        }
    }

}
