using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapLaser : MonoBehaviour
{
    public DOTweenAnimation tweener;

    [Header(" - Graphic change - ")]
    public GameObject object_3d;
    public GameObject object_2d;
    public ParticleSystem viewChange2dEffect;
    public ParticleSystem viewChange3dEffect;


    public void OnViewChangedStartTo2D()
    {
        tweener.DOPause();
    }

    public void OnViewChangedStartTo3D()
    {
        tweener.DOPause();
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

        tweener.DOPlay();
    }

    public void OnViewChangedEndTo3D()
    {
        viewChange3dEffect.Stop();
        viewChange3dEffect.gameObject.SetActive(false);

        tweener.DOPlay();
    }

}
