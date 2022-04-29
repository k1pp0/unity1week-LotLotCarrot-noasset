using System;
using Cysharp.Threading.Tasks;
using GameJamUtility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class PlayerPresenter : MonoBehaviour
{
    // Player
    [SerializeField] private Player player;
    
    // UI
    [SerializeField] private ScoreView scoreView;
    [SerializeField] private ResultView resultView;

    [SerializeField] private CameraTracker cameraTracker;
    [Inject] private IGameInputHandler gameInputHandler;
    [Inject] private IGameStateProvider gameStateProvider;

    private Camera mainCamera;
    private float cameraDistance = 10f;
    private float cameraRotate = 0f;
    private float weight = 6.0f;

    private void Start()
    {
        mainCamera = cameraTracker.GetComponent<Camera>();
        
        // プレイヤー移動
        gameInputHandler.OnScreenPositionChanged
            .Where(_ => gameStateProvider.IsGamePlay())
            .Where(_ => !player.isEating)
            .Select(p => mainCamera.ScreenPointToRay(p))
            .Do(ray => Debug.DrawRay( ray.origin, ray.direction * 1000.0f))
            .Select(ray => (Physics.Raycast(ray, out var hitInfo, 100f), hitInfo))
            .Where(hit => hit.Item1)
            .Where(hit => hit.hitInfo.transform.CompareTag("Ground"))
            .Select(hit => hit.hitInfo.point)
            .Subscribe(player.SetDestination)
            .AddTo(this);

        // ゲーム終了で食べるのを停止
        gameStateProvider.CurrentState
            .Where(s => s == GameState.GameClear)
            .First()
            .Subscribe(_ =>
            {
                player.EatFinish();
            }).AddTo(this);

        // カメラ移動
        this.UpdateAsObservable()
            .Select(_ => player.transform)
            .Subscribe(t =>
            {
                cameraTracker.SetTargetPosition(t.position);
                cameraTracker.SetDistance(Mathf.Log(t.localScale.magnitude) * cameraDistance, cameraRotate);
            })
            .AddTo(this);
        
        // カメラ回転
        gameInputHandler.OnGetAxisRaw
            .Subscribe(input =>
            {
                // cameraDistance += input.y / 10f;
                cameraRotate += input.x * Mathf.Deg2Rad;
            })
            .AddTo(this);

        player.TotalEatNumber
            .Where(_ => gameStateProvider.IsGamePlay() || gameStateProvider.IsGameClear())
            .Subscribe(n =>
            {
                weight = 6.0f + Mathf.Pow(n * 0.21f, 2);
                scoreView.SetWight(weight);
            }).AddTo(this);

        gameStateProvider.CurrentState
            .Where(s => s == GameState.GameClear)
            .First()
            .SelectMany(_ => UniTask.Delay(2000).ToObservable())
            .Subscribe(_ =>
            {
                resultView.SetResult(player.TotalEatNumber.Value);
                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(weight);
            }).AddTo(this);
    }
}