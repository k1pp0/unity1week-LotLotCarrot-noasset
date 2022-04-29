using System;

namespace GameJamUtility
{
    public interface IGameTimerProvider
    {
        IObservable<int> ReadyTime { get; }
        IObservable<int> RemainingTime { get; }
        IObservable<float> ElapsedTime { get; }
        void StartReady(int time);
        void StartGame(int time);
        void AddGameTime(int time);
        void StartTimer();
    }
}