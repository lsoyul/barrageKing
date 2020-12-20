using Cysharp.Threading.Tasks;
using Pixelplacement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{
    public class IngameTaskManager : Singleton<IngameTaskManager>
    {
        [Button]
        private async UniTask PrepareGameTask()
        {
            await MapManager.Instance.GenerateMap();
            await CharacterManager.Instance.GenerateCharacter();
        }

        private async UniTask StartGameTask()
        {

        }
    }

}
