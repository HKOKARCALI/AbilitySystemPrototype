using AbilitySystem.Core;
using UnityEngine;

namespace AbilitySystem.Manager
{

    public class GameManager : MakeSingleton<GameManager>
    {
        public GameState State { get; private set; } = GameState.NONE;

        public delegate void GameStateChangeEvent(GameState oldState, GameState newState);
        public static GameStateChangeEvent OnGameStateChange;

        void Start()
        {
            SetState(GameState.MENU);
        }

        void Update()
        {
            
        }

        #region setgamestate
        public void SetState(GameState newState)
        {
            GameState oldState = State;
            State = newState;
            OnGameStateChange?.Invoke(oldState, State);
        }

        #endregion
        public enum GameState
        {
            NONE, MENU, PLAY, PAUSE, TRANSITION,
        }

       
    }
}
