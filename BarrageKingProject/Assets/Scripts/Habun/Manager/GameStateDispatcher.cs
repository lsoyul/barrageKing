using UnityAtoms.FSM;
using UnityEngine;

namespace Habun
{
    public class GameStateDispatcher : MonoSingleton<GameStateDispatcher>
    {
        public static readonly string GAMESTART = "GameStart";
        public static readonly string GAMEOVER = "GameOver";

        [SerializeField]
        private FiniteStateMachineReference gameState;

        // PROPERTIES: ---------------------------------------------------------

        public FiniteStateMachine GameState
        {
            get { return gameState.Machine; }
        }

        // PUBLIC METHODS: ----------------------------------------------------

        public void DispatchGameStart(GameObject player)
        {
            if (player != null)
            {
                GameState.Dispatch(command: GAMESTART);
            }
        }

        public void DispatchGameOver(float health)
        {
            if (health <= 0.0f)
            {
                GameState.Dispatch(command: GAMEOVER);
            }
        }

    }
}
