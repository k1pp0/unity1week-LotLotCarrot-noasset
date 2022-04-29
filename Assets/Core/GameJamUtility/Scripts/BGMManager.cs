using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private Coroutine fadeCoroutine;
    public float maxValue;
    private float rate = 0.5f;
    
    public void FadeIn(float time)
    {
        audioSource.DOFade(maxValue * rate, time);
    }
    
    public void FadeOut(float time, float to)
    {
        audioSource.DOFade(to * rate, time);
    }

    public void SetPitch(float p)
    {
        audioSource.DOPitch(p, 0.5f);
    }
    
    public void SetVolumeRate(float r)
    {
        audioSource.volume = maxValue * rate;
        rate = r;
    }
}