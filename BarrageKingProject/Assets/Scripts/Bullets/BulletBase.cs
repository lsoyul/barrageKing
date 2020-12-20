using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Rendering;
using NUnit.Framework.Constraints;
using Adohi;
using Sirenix.OdinInspector.Editor;
using MoreLinq.Extensions;

public class BulletBase : MonoBehaviour
{
    public GameStatics.BULLET_TYPE bulletType = GameStatics.BULLET_TYPE.STONE;
    public float speed = 1f;
    public float acceleration = 0f;
    public float radius = 5f;                   // Collider radius

    public float maxLivingDuration = 1000f;          
    public float maxLivingDistance = 500f;

    [Header(" - Graphic change - ")]
    public GameObject object_3d;
    public GameObject object_2d;
    public ParticleSystem viewChange2dEffect;
    public ParticleSystem viewChange3dEffect;


    public List<TrailRenderer> trailRenderer;

    private float curTimer = 0;
    private Vector3 curVelocity = Vector3.zero;

    private Vector3 tempVelocity = Vector3.zero;

    [SerializeField] private bool isFire = false;
    [SerializeField] private Vector3 initPosition = Vector3.zero;

    public Action<BulletBase> onDestroy;

    public Action<GameObject, Collision> onCollision;
    bool isCheckCollide = true;

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

        if (ViewPointManager.Instance != null)
        {
            ViewPointManager.Instance.OnViewChangedStartTo2D += OnViewChangedStartTo2D;
            ViewPointManager.Instance.OnViewChangedStartTo3D += OnViewChangedStartTo3D;
            ViewPointManager.Instance.OnViewChangedMiddleTo2D += OnViewChangedMiddleTo2D;
            ViewPointManager.Instance.OnViewChangedMiddleTo3D += OnViewChangedMiddleTo3D;
            ViewPointManager.Instance.OnViewChangedEndTo2D += OnViewChangedEndTo2D;
            ViewPointManager.Instance.OnViewChangedEndTo3D += OnViewChangedEndTo3D;

            SetObjectDimensional();
        }
        
        viewChange2dEffect.gameObject.SetActive(false);
        viewChange3dEffect.gameObject.SetActive(false);

        isCheckCollide = true;
    }

    void SetObjectDimensional()
    {
        switch (ViewPointManager.Instance.currentViewPoint)
        {
            case ViewPoint.twoDimensional:
                object_2d.SetActive(true);
                object_3d.SetActive(false);
                break;
            case ViewPoint.threeDimensional:
                object_2d.SetActive(false);
                object_3d.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void OnViewChangedStartTo2D()
    {
        tempVelocity = curVelocity;
        curVelocity = Vector3.zero;
    }

    public void OnViewChangedStartTo3D()
    {
        tempVelocity = curVelocity;
        curVelocity = Vector3.zero;
    }

    public void OnViewChangedMiddleTo2D()
    {
        object_2d.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        object_2d.SetActive(true);
        object_3d.SetActive(false);

        viewChange2dEffect.gameObject.SetActive(true);
        viewChange2dEffect.Play();
    }

    public void OnViewChangedMiddleTo3D()
    {
        object_3d.SetActive(true);
        object_2d.SetActive(false);

        viewChange3dEffect.gameObject.SetActive(true);
        viewChange3dEffect.Play();

    }

    public void OnViewChangedEndTo2D()
    {
        viewChange2dEffect.Stop();
        viewChange2dEffect.gameObject.SetActive(false);
        curVelocity = tempVelocity;
    }

    public void OnViewChangedEndTo3D()
    {
        viewChange3dEffect.Stop();
        viewChange3dEffect.gameObject.SetActive(false);
        curVelocity = tempVelocity;
    }


    protected virtual void Update()
    {
        if (isFire)
        {
            if (Mathf.Approximately(acceleration, 0f) == false) speed += acceleration;
            
            curVelocity = this.transform.forward * speed;

            this.transform.position += curVelocity * Time.deltaTime;

            if (ViewPointManager.Instance != null)
            {
                if (ViewPointManager.Instance.isViewChanging == false)
                    curTimer += Time.deltaTime;
            }
            else
            {
              curTimer += Time.deltaTime;
            }

            if (curTimer > maxLivingDuration
                || Vector3.Distance(this.gameObject.transform.position, this.initPosition) > maxLivingDistance)
            {
                // if (living time over) or (far away from max distance) than return to datapool

                OnDestroyBullet();
            }
        }
    }


    public void OnDestroyBullet()
    {
        isFire = false;
        isCheckCollide = false;

        if (ViewPointManager.Instance != null)
        {
            ViewPointManager.Instance.OnViewChangedStartTo2D -= OnViewChangedStartTo2D;
            ViewPointManager.Instance.OnViewChangedStartTo3D -= OnViewChangedStartTo3D;
            ViewPointManager.Instance.OnViewChangedMiddleTo2D -= OnViewChangedMiddleTo2D;
            ViewPointManager.Instance.OnViewChangedMiddleTo3D -= OnViewChangedMiddleTo3D;
            ViewPointManager.Instance.OnViewChangedEndTo2D -= OnViewChangedEndTo2D;
            ViewPointManager.Instance.OnViewChangedEndTo3D -= OnViewChangedEndTo3D;

            if (trailRenderer != null && trailRenderer.Count > 0)
            {
                foreach (var trailer in trailRenderer)
                {
                    trailer.Clear();
                }
            }


            if (onDestroy != null) onDestroy(this);
        }
    }

    public void SetIsCheckCollide(bool isCheck)
    {
        this.isCheckCollide = isCheck;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isCheckCollide)
            onCollision?.Invoke(this.gameObject, collision);
    }

}
