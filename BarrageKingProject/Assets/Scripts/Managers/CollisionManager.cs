using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoSingleton<CollisionManager>
{

    protected override void OnInitialize()
    {
        base.OnInitialize();

        // Add Callbacks

    }

    public void OnCollideWithObject(GameObject detectedObj, Collider targetObj)
    {
        if (detectedObj.CompareTag("Bullet"))
        {
            if (targetObj.gameObject.CompareTag("Wall"))
            {
                // - Bullet vs Wall
                BulletBase bullet = detectedObj.gameObject.GetComponentInParent<BulletBase>();
                if (bullet != null)
                {
                    // remove bullet
                    bullet.OnDestroyBullet();
                }
            }
            else if (targetObj.gameObject.CompareTag("Player"))
            {
                // - Player vs Bullet
                BulletBase bullet = detectedObj.gameObject.GetComponentInParent<BulletBase>();

                if (bullet != null)
                {
                    // player damaged
                    float damage = GameStatics.GetBulletDamage(bullet.bulletType);

                    // remove bullet
                    bullet.OnDestroyBullet();
                }
            }
        }
    }

}
