using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public IObservable<Unit> OnAte => onAte;
    private readonly Subject<Unit> onAte = new Subject<Unit>();
    public IObservable<Vector3> OnGrowth => onGrowth;
    private readonly Subject<Vector3> onGrowth = new Subject<Vector3>();
    private ParticleSystem ps => psCache ? psCache : psCache = GetComponent<ParticleSystem>();
    private ParticleSystem psCache;
    private IDisposable timer;
    
    public void Plant(Vector3 plantPosition, float growthTime)
    {
        if (ps.isPlaying) ps.Stop();
        var main = ps.main;
        main.duration = growthTime;
        main.startLifetime = growthTime;
        transform.position = plantPosition;
        ps.Play();
        
        timer = Observable.Interval(TimeSpan.FromSeconds(growthTime))
            .Subscribe(_ => { onGrowth.OnNext(plantPosition); }).AddTo(this);
    }

    public void Ate()
    {
        ps.Stop();
        timer.Dispose();
        onAte.OnNext(Unit.Default);
    }
}