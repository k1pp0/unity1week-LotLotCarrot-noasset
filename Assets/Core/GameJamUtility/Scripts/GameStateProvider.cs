using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameJamUtility
{
    public class GameStateProvider : IGameStateProvider, IInitializable, IDisposable
    {
        private readonly ReactiveProperty<GameState> currentState = new ReactiveProperty<GameState>();
        IObservable<GameState> IGameStateProvider.CurrentState => currentState;
        private readonly CompositeDisposable disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            currentState.AddTo(disposable);
        }

        bool IGameStateProvider.IsGameReady()
        {
            return currentState.Value == GameState.GameReady;
        }

        bool IGameStateProvider.IsGamePlay()
        {
            return currentState.Value == GameState.GamePlay;
        }

        bool IGameStateProvider.IsGameOver()
        {
            return currentState.Value == GameState.GameOver;
        }

        bool IGameStateProvider.IsGameClear()
        {
            return currentState.Value == GameState.GameClear;
        }

        bool IGameStateProvider.IsGameFinish()
        {
            return currentState.Value == GameState.GameFinish;
        }

        void IGameStateProvider.SetState(GameState state)
        {
            currentState.Value = state;
        }

        void IDisposable.Dispose()
        {
            disposable.Dispose();
        }
    }
    public enum GameState
    {
        GameReady,
        GamePlay,
        GameOver,
        GameClear,
        GameFinish
    }
}
