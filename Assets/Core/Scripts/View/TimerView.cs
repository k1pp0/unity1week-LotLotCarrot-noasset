using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    [SerializeField] private Image[] timerImages;

    public void StartCountDown(int index, float time)
    {
        timerImages[index].DOFillAmount(0f, time)
            .SetEase(Ease.Linear);
    }
}
