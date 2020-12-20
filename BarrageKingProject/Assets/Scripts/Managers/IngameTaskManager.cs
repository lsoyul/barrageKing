using Cysharp.Threading.Tasks;
using Pixelplacement;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{
    public class IngameTaskManager : Singleton<IngameTaskManager>
    {
        public event Action OnStartGame;

        [Button]
        private async UniTask PrepareGameTask()
        {
            await MapManager.Instance.GenerateMap();
            await UniTask.Delay(1000);
            await CharacterManager.Instance.GenerateCharacter();

            await StartGameTask();
        }

        private async UniTask StartGameTask()
        {

            OnStartGame?.Invoke();

        }
    }

}
