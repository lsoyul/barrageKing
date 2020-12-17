using UnityAtoms.BaseAtoms;
using UnityAtoms.FSM;
using UnityEngine;

namespace Habun
{
    public class GameStateDispatcher : MonoSingleton<GameStateDispatcher>
    {
        [SerializeField]
        private FiniteStateMachineReference gameState;

        [SerializeField,Space]
        private StringUnityEvent onState;

        // PROPERTIES: ---------------------------------------------------------

        public FiniteStateMachine GameState
        {
            get { return gameState.Machine; }
        }

        // MONOBEHAVIOUR METHODS: ----------------------------------------------------

        private void OnEnable()
        {
            gameState.Machine.Changed.Register(OnState);
        }

        private void OnDisable()
        {
            gameState.Machine.Changed.Unregister(OnState);
        }

        // CALLBACK METHODS: ----------------------------------------------------

        private void OnState(string state)
        {
            onState.Invoke(state);
        }

    }
}
