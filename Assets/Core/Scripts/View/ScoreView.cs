using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private Text weightText;

    public void SetWight(float w)
    {
        weightText.text = $"{w:N2} kg";
        weightText.transform.DOScale(Vector3.one * 1.2f, 0.1f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() => 
                weightText.transform.DOScale(Vector3.one, 0.1f));
    }
}
