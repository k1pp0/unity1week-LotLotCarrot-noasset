using GameJamUtility;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class ResultView : MonoBehaviour
{
    [Inject] private ISceneTransitionManager sceneTransitionManager;
    [Inject] private SEManager seManager;

    [SerializeField] private Button tweetButton;
    [SerializeField] private Button rankingButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Text resultText;
    [SerializeField] private Text nameText;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup buttonsCanvasGroup;

    private int totalEatNumber;
    private string pigName;
    private float weight;

    private void Start()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        
        backButton.OnClickAsObservable()
            .First()
            .Subscribe(_ =>
            {
                seManager.PlayPig();
                sceneTransitionManager.Run("Title", 1, false);
            }).AddTo(this);

        rankingButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                SceneManager.LoadScene("Ranking", LoadSceneMode.Additive);
            }).AddTo(this);
        
        // Tweet
        tweetButton.OnClickAsObservable()
            .Subscribe(async _ =>
            {
                buttonsCanvasGroup.alpha = 0;
                var text= $"「{pigName}」は「{totalEatNumber}本」のにんじんを食べて「{weight:N2} kg」に育った！";
                await StartCoroutine(TweetWithScreenShot.TweetManager.TweetWithScreenShot(text));
                buttonsCanvasGroup.alpha = 1;
            }).AddTo(this);
    }

    public void SetResult(int n)
    {
        totalEatNumber = n;
        pigName = PlayerPrefs.GetString("PIG_NAME");
        weight = 6.0f + Mathf.Pow(n * 0.21f, 2);
        resultText.text = $"{totalEatNumber} ほんの にんじんを \nたべてそだった！";
        nameText.text = pigName;
    }

    public void Show()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
    }
}