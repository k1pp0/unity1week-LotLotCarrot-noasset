using DG.Tweening;
using GameJamUtility;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TitlePresenter : MonoBehaviour
{
    // View
    [SerializeField] private Button startButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Text nameText;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Transform soundSettings;

    // Model
    [Inject] private ISceneTransitionManager sceneTransitionManager;
    [Inject] private BGMManager bgmManager;
    [Inject] private SEManager seManager;

    private Tween tween;
    private string pigName;

    private void Start()
    {
        bgmManager.FadeIn(2.0f);

        pigName = PlayerPrefs.GetString("PIG_NAME");
        nameInputField.text = pigName;
        
        nameText.ObserveEveryValueChanged(t => t.text)
            .Where(t => t.Length > 0)
            .Subscribe(_ =>
            {
                startButton.interactable = true;
                
                tween?.Kill();
                tween = startButton.transform.DORotate(Vector3.zero, 0.3f)
                    .SetEase(Ease.OutBounce);
            }).AddTo(this);
        
        nameText.ObserveEveryValueChanged(t => t.text)
            .Where(t => t.Length <= 0)
            .Subscribe(_ =>
            {
                startButton.interactable = false;
                
                tween?.Kill();
                tween = startButton.transform.DORotate(Vector3.up * 90f, 0.3f);
            }).AddTo(this);

        startButton.OnClickAsObservable()
            .First()
            .Subscribe(_ =>
            {
                seManager.PlayPig();
                PlayerPrefs.SetString("PIG_NAME", nameText.text);
                bgmManager.FadeOut(2.0f, 0.1f);
                sceneTransitionManager.Run("Main", 1, false);
            }).AddTo(this);

        // 音量設定
        soundButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                soundSettings.DOScale(1.0f, 0.1f);
            }).AddTo(this);
        closeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                soundSettings.DOScale(0.0f, 0.1f);
            }).AddTo(this);
        bgmSlider.OnValueChangedAsObservable()
            .Subscribe(bgmManager.SetVolumeRate)
            .AddTo(this);
        seSlider.OnValueChangedAsObservable()
            .Subscribe(seManager.SetVolumeRate)
            .AddTo(this);
    }
}