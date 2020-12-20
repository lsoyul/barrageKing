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
        if (detectedObj.layer == LayerMask.NameToLayer("Bullet"))
        {
            BulletBase bullet = detectedObj.gameObject.GetComponentInParent<BulletBase>();

            if (targetObj.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                // Bullet vs Wall
                if (bullet != null)
                {
                    // remove bullet
                    bullet.OnDestroyBullet();
                }
            }
            else if (targetObj.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Bullet vs Player

                if (bullet != null)
                {
                    // player damaged
                    float damage = GameStatics.GetBulletDamage(bullet.bulletType);

                    // remove bullet
                    bullet.OnDestroyBullet();
                }
            }
            else if (targetObj.gameObject.layer == LayerMask.NameToLayer("Box"))
            {
                // Bullet vs Box
                if (bullet != null)
                {
                    // remove bullet
                    bullet.OnDestroyBullet();
                }
            }
        }
    }

}
