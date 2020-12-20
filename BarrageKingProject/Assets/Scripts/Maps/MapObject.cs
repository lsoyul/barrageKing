using DG.Tweening;
using PD.UnityEngineExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Adohi
{
    public enum MapObjectType
    {
        Ground,
        Wall,
        Obstacle,
        Box
    }
    


    public class MapObject : MonoBehaviour
    {
        private Color startColor;
        private Color endColor;
        private Material material;
        public MapObjectType objectType;
        public float timeing = 0.6f;
        public Color defalutColor;

        [Header("2D Setting")]
        public GameObject object2D;
        public bool isMoveAvailable;

        [Header("3D Setting")]
        public GameObject object3D;
        public Renderer object3DMaterial;
        public VisualEffect revealVFX;
        public VisualEffect hidedVFX;
        public float height;
        public int particleCountPerHeight = 200;
        public float growDuration;
        public float currentFadeDuration;

        private void Awake()
        {
            material = object3D.GetComponentInChildren<Renderer>().material;
        }
        private void Start()
        {
            startColor = defalutColor;
            Color.RGBToHSV(defalutColor, out var h, out var s, out var v);
            endColor = Color.HSVToRGB(h, 1f / height * Random.Range(0.8f, 1.2f), v);
            ViewPointManager.Instance.OnViewChangedStartTo2D += () => Hide3DObject();
            ViewPointManager.Instance.OnViewChangedMiddleTo2D += () => Show2DObject();
            ViewPointManager.Instance.OnViewChangedStartTo3D += () => Hide2DObject();
            ViewPointManager.Instance.OnViewChangedMiddleTo3D += () => Show3DObject();
        }

        private void Update()
        {
            if (currentFadeDuration > 0f)
            {
                var color = this.material.GetColor("_BaseColor");
                this.material.SetColor("_BaseColor", new Color(color.r, color.g, color.b, 0.4f));
                currentFadeDuration -= Time.deltaTime;
            }
            else
            {
                var color = this.material.GetColor("_BaseColor");
                this.material.SetColor("_BaseColor", new Color(color.r, color.g, color.b, 1f));
                currentFadeDuration = 0f;
            }
        }

        [Button]
        public void Show3DObject()
        {
            revealVFX.SetVector4("startColor", startColor);
            revealVFX.SetVector4("endColor", endColor);
            revealVFX.SetInt("spawnCount", (height * particleCountPerHeight).ToInt());
            revealVFX.SetFloat("height", height);
            revealVFX.SetFloat("spawnDuration", growDuration * timeing);
            this.revealVFX.enabled = false;
            this.object3D.SetActive(false);
            this.revealVFX.enabled = true;
            this.object3D.SetActive(true);
            this.object3D.transform.DOScaleY(height, growDuration).From(0f);
        }

        [Button]
        public void Hide3DObject()
        {
            hidedVFX.SetVector4("startColor", endColor);
            hidedVFX.SetVector4("endColor", startColor);
            hidedVFX.SetInt("spawnCount", (height * particleCountPerHeight).ToInt());
            hidedVFX.SetFloat("height", height);
            hidedVFX.SetFloat("spawnDuration", growDuration * timeing);
            this.hidedVFX.enabled = false;
            this.object3D.SetActive(false);
            this.hidedVFX.enabled = true;
            this.object3D.SetActive(true);
            this.object3D.transform.DOScaleY(0f, growDuration).From(height).OnComplete(() => this.object3D.SetActive(false));
        }

        public void Show2DObject()
        {
            this.revealVFX.enabled = false;
            this.object3D.SetActive(false);
            this.object2D.SetActive(true);
            this.object2D.transform.DOScale(1f, 1f).From(0f).SetDelay(Random.Range(0f, 0.5f));
        }

        public void Hide2DObject()
        {
            this.object2D.transform.DOScale(0f, 1f).From(1f).SetDelay(Random.Range(0f, 0.5f)).OnComplete(() => this.object2D.SetActive(false));
        }


        public void InitObject()
        {
            this.revealVFX.enabled = false;
            this.object3D.SetActive(false);
            this.object2D.SetActive(false);
        }
        

        public void DoFade3D()
        {

        }
    }

}
