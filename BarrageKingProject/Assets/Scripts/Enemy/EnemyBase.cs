using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float radius = 5f;
    public float shootDelay = 0.4f;       // sec

    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > shootDelay)
        {
            BulletManager.Instance().FireBullet(GameStatics.BULLET_TYPE.NORMAL1, this.transform.position, radius, 1, 1, this.transform.forward);
            timer = 0;
        }


        this.transform.Rotate(Vector3.up, Space.Self);
    }

}
