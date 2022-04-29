using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEManager : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip eat;
    [SerializeField] private AudioClip pi0;
    [SerializeField] private AudioClip pi1;
    [SerializeField] private AudioClip pi2;
    [SerializeField] private AudioClip pig;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayEat()
    {
        audioSource.PlayOneShot(eat);
    }

    public void PlayCountDown()
    {
        audioSource.PlayOneShot(pi0);
    }

    public void PlayStart()
    {
        audioSource.PlayOneShot(pi1);
    }

    public void PlayFinish()
    {
        audioSource.PlayOneShot(pi2);
    }
    
    public void PlayPig()
    {
        audioSource.PlayOneShot(pig);
    }

    public void SetVolumeRate(float r)
    {
        audioSource.volume = r;
    }
}