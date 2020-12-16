using UnityAtoms.BaseAtoms;
using UnityAtoms.FSM;
using UnityEngine;

namespace Habun
{
    public class GameStateManager : MonoSingleton<GameStateManager>
    {
        [SerializeField]
        private FiniteStateMachineReference gameStateMachine;

    }
}
