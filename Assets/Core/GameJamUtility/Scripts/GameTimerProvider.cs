using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameJamUtility
{
    public class GameTimerProvider : IGameTimerProvider, IInitializable, IDisposable, ITickable
    {
        private IDisposable timer;
        private float startTime;

        private readonly ReactiveProperty<int> readyTime = new ReactiveProperty<int>(-1);
        IObservable<int> IGameTimerProvider.ReadyTime => readyTime;

        private readonly ReactiveProperty<int> remainingTime = new ReactiveProperty<int>(-1);
        IObservable<int> IGameTimerProvider.RemainingTime => remainingTime;
        
        private readonly ReactiveProperty<float> elapsedTime = new ReactiveProperty<float>(-1);
        IObservable<float> IGameTimerProvider.ElapsedTime => elapsedTime;

        private readonly CompositeDisposable disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            readyTime.AddTo(disposable);
            remainingTime.AddTo(disposable);
            elapsedTime.AddTo(disposable);
        }

        void ITickable.Tick()
        {
            if (Math.Abs(startTime) < 0.0001f) return;
            elapsedTime.Value = Time.time - startTime;
        }

        void IDisposable.Dispose()
        {
            disposable.Dispose();
        }

        void IGameTimerProvider.StartReady(int time)
        {
            timer = CreateTimer(time)
                .Subscribe(t => readyTime.Value = t)
                .AddTo(disposable);
        }

        void IGameTimerProvider.StartGame(int time)
        {
            timer = CreateTimer(time)
                .Subscribe(t => remainingTime.Value = t)
                .AddTo(disposable);
        }

        void IGameTimerProvider.AddGameTime(int time)
        {
            timer = CreateTimer(remainingTime.Value + time)
                .Subscribe(t => remainingTime.Value = t)
                .AddTo(disposable);
        }

        private IObservable<int> CreateTimer(int time)
        {
            timer?.Dispose();
            return Observable.Interval(TimeSpan.FromSeconds(1))
                .Select(i => (int) (time - i))
                .TakeWhile(t => t >= 0);
        }

        void IGameTimerProvider.StartTimer()
        {
            startTime = Time.time;
        }
    }
}