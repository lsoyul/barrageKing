using Cysharp.Threading.Tasks;
using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        public CharacterSpawner spawner;
        public Location initialLocation;
        public Character character;

        public async UniTask GenerateCharacter()
        {
            this.character = await spawner.Spawn(initialLocation);
        }
    }

}
