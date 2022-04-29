using System;
using GameJamUtility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class CarrotPresenter : MonoBehaviour
{
    // View
    [SerializeField] private Player player;
    [SerializeField] private Carrot prefab;
    [SerializeField] private Transform hierarchyTransform;

    // Model
    [Inject] private IGameStateProvider gameStateProvider;

    private CarrotPool carrotPool;
    public int preloadCarrotMax;
    private int carrotMax = 1;
    private int carrotRent = 0;
    private int plantNumber = 3;
    private float randomRange = 15.0f;
    private float growthRange = 3.0f;
    private float growthTime = 5.0f;
    private float plantTime = 3.0f;

    public bool isTitle;

    private void Start()
    {
        carrotPool = new CarrotPool(hierarchyTransform, prefab);
        carrotPool.PreloadAsync(preloadCarrotMax, 1)
            .Subscribe(_ => Debug.Log("preload finished"))
            .AddTo(this);
        this.OnDestroyAsObservable()
            .Subscribe(_ => carrotPool.Dispose())
            .AddTo(this);

        Plant(Vector3.zero, randomRange);
        Observable.Interval(TimeSpan.FromSeconds(plantTime))
            .Where(_ => gameStateProvider.IsGamePlay() || isTitle)
            .Subscribe(_ =>
            {
                for (var i = 0; i < plantNumber; i++)
                {
                    Plant(Vector3.zero, randomRange);
                }
            })
            .AddTo(this);

        if (!isTitle)
        {
            player.TotalEatNumber.Subscribe(n => { carrotMax = (n + 1) * 2; }).AddTo(this);
        }
    }

    private void Plant(Vector3 plantPosition, float range)
    {
        if (carrotRent >= carrotMax) return;

        var position = plantPosition +
                       new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        var carrot = carrotPool.Rent();
        carrotRent++;
        carrot.OnAte
            .Where(_ => gameStateProvider.IsGamePlay() || isTitle)
            .Subscribe(c =>
            {
                carrotPool.Return(carrot);
                carrotRent--;
            }).AddTo(this);
        carrot.OnGrowth
            .Where(_ => gameStateProvider.IsGamePlay() || isTitle)
            .Subscribe(p => { Plant(p, growthRange); }).AddTo(this);
        carrot.Plant(position, growthTime);

        if (isTitle)
        {
            player.SetDestination(position);
        }
    }
}