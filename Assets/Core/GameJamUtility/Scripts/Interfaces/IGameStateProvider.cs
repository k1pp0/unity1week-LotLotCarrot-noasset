using System;

namespace GameJamUtility
{
    public interface IGameStateProvider
    {
        IObservable<GameState> CurrentState { get; }
        void SetState(GameState state);
        bool IsGameReady();
        bool IsGamePlay();
        bool IsGameOver();
        bool IsGameClear();
        bool IsGameFinish();
    }
}