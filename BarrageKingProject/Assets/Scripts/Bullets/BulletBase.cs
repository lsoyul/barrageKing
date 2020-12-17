using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Rendering;

public class BulletBase : MonoBehaviour
{
    public GameStatics.BULLET_TYPE bulletType = GameStatics.BULLET_TYPE.NORMAL1;
    public Vector3 velocity = Vector3.one;
    public Vector3 acceleration = Vector3.zero;
    public float radius = 5f;                   // Collider radius

    public float maxLivingDuration = 10f;          
    public float maxLivingDistance = 500f;

    private float curTimer = 0;
    private Vector3 curVelocity = Vector3.zero;

    private bool isFire = false;
    private Vector3 initPosition = Vector3.zero;

    Action<BulletBase> onDestroy;

    public Vector3 GetDirection()
    {
        return this.transform.forward;
    }


    protected virtual void Fire(GameStatics.BULLET_TYPE bulletType, float radius, Vector3 velocity, Vector3 acceleration, Vector3 direction)
    {
        this.bulletType = bulletType;
        this.radius = radius;
        this.velocity = velocity;
        this.acceleration = acceleration;

        Vector3 lookAtPoint = this.transform.position;
        lookAtPoint += direction.normalized;

        this.transform.LookAt(lookAtPoint);

        this.curVelocity = velocity;
        this.curTimer = 0;
        this.initPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

        this.isFire = true;
    }

    protected virtual void Update()
    {
        if (isFire)
        {
            curVelocity += acceleration;

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
