using Cysharp.Threading.Tasks;
using DG.Tweening;
using PD.UnityEngineExtensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Adohi
{
    public class OpeningSceneManager : MonoBehaviour
    {
        public Camera openingCamera;
        public Color firstColor;
        public Color secondColor;
        public Color thirdColor;
        [Header("Cube")]
        public GameObject cube;
        public Vector3 finalCubePosition;

        [Header("Text")]
        public TextMeshPro title;
        public TextMeshPro press;
        public TMP_FontAsset secondFont;
        public Vector3 finalTitlePosition;

        [Header("Delay")]
        public float firstDelay;
        public float firstrotateDuration;
        public float secondDelay;
        public float secondrotateDuration;
        public float thirdDelay;
        public float fourthDelay;
        public float fourthrotateDuration;


        private void Start()
        {
            openingCamera.backgroundColor = firstColor;
            StartOpeningTask();
        }

        public async UniTask StartOpeningTask()
        {
            await UniTask.Delay((firstDelay * 1000).ToInt());
            await UniTask.WhenAll
            (
                RotateObject(cube, Quaternion.Euler(45f, 0f, 0f), firstrotateDuration),
                RotateObject(title.gameObject, Quaternion.Euler(45f, 0f, 0f), firstrotateDuration),
                ChangeColor(openingCamera, secondColor, firstrotateDuration)
            );
            await UniTask.Delay((secondDelay * 1000).ToInt());
            await UniTask.WhenAll
            (
                RotateObject(cube, Quaternion.Euler(45f, 0f, 45f), secondrotateDuration),
                RotateObject(title.gameObject, Quaternion.Euler(45f, 0f, -45f), secondrotateDuration),
                ChangeColor(openingCamera, thirdColor, secondrotateDuration)
            );
            await UniTask.Delay((thirdDelay * 1000).ToInt());
            title.font = secondFont;
            await UniTask.Delay((fourthDelay * 1000).ToInt());
            await UniTask.WhenAll
            (
                MoveObject(cube, finalCubePosition, fourthrotateDuration),
                MoveObject(title.gameObject, finalTitlePosition, fourthrotateDuration)
            );
            press.gameObject.SetActive(true);
        }

        public async UniTask MoveObject(GameObject gameObject, Vector3 position, float duration)
        {
            await gameObject.transform.DOMove(position, duration).AsyncWaitForCompletion();
        }


        public async UniTask RotateObject(GameObject gameObject, Quaternion quaternion, float duration)
        {
            await gameObject.transform.DORotateQuaternion(quaternion, duration).AsyncWaitForCompletion();
        }

        public async UniTask ChangeColor(Camera camera, Color backgoundColor, float duration)
        {
            Color.RGBToHSV(camera.backgroundColor, out var h, out var s, out var v);
            Color.RGBToHSV(backgoundColor, out var toH, out var toS, out var toV);

            float currentTime = 0f;
            while (currentTime < duration)
            {
                camera.backgroundColor = Color.HSVToRGB(
                    Mathf.Lerp(h, toH, currentTime / duration),
                    Mathf.Lerp(s, toS, currentTime / duration),
                    Mathf.Lerp(v, toV, currentTime / duration));
                await UniTask.DelayFrame(1);
                currentTime += Time.deltaTime;
            }

            camera.backgroundColor = backgoundColor;
        }

    }

}
