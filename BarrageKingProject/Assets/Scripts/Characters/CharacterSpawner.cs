using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{
    public class CharacterSpawner : MonoBehaviour
    {
        public Character characterPrefab;

        public async UniTask<Character> Spawn(Location location)
        {
            var character = Instantiate(characterPrefab);
            character.currentLocation = location;
            character.SyncPosition();
            character.ConnectCamera();
            return character;
        }
    }

}
