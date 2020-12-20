using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHp : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public DOTweenAnimation tweener;

    public void SetHP(float current)
    {
        hpText.text = "HP : " + (int)current;
        tweener.DOPlay();
    }
}
