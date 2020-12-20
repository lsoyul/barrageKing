using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoSingleton<CollisionManager>
{

    public void OnCollideWithObject(GameObject detectedObj, Collision targetObj)
    {
        if (detectedObj.CompareTag("Player"))
        {
            if (targetObj.gameObject.CompareTag("Bullet"))
            {
                // - Player vs Bullet
                BulletBase bullet = targetObj.gameObject.GetComponentInParent<BulletBase>();

                if (bullet != null)
                {
                    // player damaged
                    float damage = GameStatics.GetBulletDamage(bullet.bulletType);

                    // remove bullet
                    bullet.OnDestroyBullet();
                }
            }
        }
        else if (detectedObj.CompareTag("Bullet"))
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
        }
    }

}
