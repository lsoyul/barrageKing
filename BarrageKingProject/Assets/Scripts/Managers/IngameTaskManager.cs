using Cysharp.Threading.Tasks;
using DigitalRuby.SoundManagerNamespace;
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

        private void Start()
        {
            PrepareGameTask();
        }

        [Button]
        private async UniTask PrepareGameTask()
        {
            SoundManager.PlayLoopingMusic(SoundContainer.Instance().BackGroundMusicsDic["BGM_Stage1"], 1, 1, true);

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
