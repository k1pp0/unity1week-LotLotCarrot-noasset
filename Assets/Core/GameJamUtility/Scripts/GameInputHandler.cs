using System;
using GameJamUtility;
using UniRx;
using UnityEngine;
using Zenject;

public class GameInputHandler : IGameInputHandler, IInitializable, IDisposable, ITickable
{

    IObservable<Unit> IGameInputHandler.OnSpacePressed => onSpacePressed;
    private Subject<Unit> onSpacePressed = new Subject<Unit>();
    IObservable<Unit> IGameInputHandler.OnEscapePressed => onEscapePressed;
    private Subject<Unit> onEscapePressed = new Subject<Unit>();
    IObservable<Vector2> IGameInputHandler.OnScreenTapped => onScreenTapped;
    private Subject<Vector2> onScreenTapped = new Subject<Vector2>();
    IObservable<Vector2> IGameInputHandler.OnScreenPositionChanged => onScreenPositionChanged;
    private Subject<Vector2> onScreenPositionChanged = new Subject<Vector2>();
    IObservable<Vector2> IGameInputHandler.OnGetAxisRaw => onGetAxisRaw;
    private Subject<Vector2> onGetAxisRaw = new Subject<Vector2>();

    private readonly CompositeDisposable disposable = new CompositeDisposable();

    void IInitializable.Initialize()
    {
    }

    void IDisposable.Dispose()
    {
        disposable.Dispose();
    }

    void ITickable.Tick()
    {
#if UNITY_IOS
        if (Input.GetMouseButtonDown(0))
#else
        if (Input.GetKeyDown(KeyCode.Space))
#endif
        {
            onSpacePressed.OnNext(Unit.Default);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onEscapePressed.OnNext(Unit.Default);
        }
        
        onScreenPositionChanged.OnNext(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            onScreenTapped.OnNext(Input.mousePosition);
        }
        onGetAxisRaw.OnNext(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }
}