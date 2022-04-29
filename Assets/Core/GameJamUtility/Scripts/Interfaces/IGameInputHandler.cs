using System;
using UniRx;
using UnityEngine;

namespace GameJamUtility
{
    public interface IGameInputHandler
    {
        IObservable<Unit> OnSpacePressed { get; }
        IObservable<Unit> OnEscapePressed { get; }

        IObservable<Vector2> OnScreenTapped { get; }
        IObservable<Vector2> OnScreenPositionChanged { get; }
        IObservable<Vector2> OnGetAxisRaw { get; }
        
    }
}
