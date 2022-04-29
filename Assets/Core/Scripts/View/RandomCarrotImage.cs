using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomCarrotImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] carrots;

    private void Start()
    {
        image.sprite = carrots[Random.Range(0, carrots.Length)];
        Observable.Interval(TimeSpan.FromSeconds(3.0f))
            .SelectMany(_ => UniTask.Delay(Random.Range(0, 1000)).ToObservable())
            .Subscribe(_ =>
            {
                transform.DOShakeScale(0.3f, 0.3f);
            }).AddTo(this);
    }
}
