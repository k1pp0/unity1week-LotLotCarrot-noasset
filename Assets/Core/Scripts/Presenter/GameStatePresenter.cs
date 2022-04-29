using GameJamUtility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Core.Scripts.Presenter
{
    public class GameStatePresenter : MonoBehaviour
    {
        [Inject] private IGameInputHandler gameInputHandler;
        [Inject] private IGameStateProvider gameStateProvider;
        [Inject] private IGameTimerProvider gameTimerProvider;
        [Inject] private ISceneTransitionManager sceneTransitionManager;
        [Inject] private BGMManager bgmManager;
        [Inject] private SEManager seManager;
        [SerializeField] private TimerView timerView;
        [SerializeField] private ResultView resultView;

        private const int GamerReadyTime = 4;
        private const int GamePlayTime = 60;

        private void Start()
        {
            gameStateProvider.SetState(GameState.GameReady);

            // Ready -> play
            gameTimerProvider.StartReady(GamerReadyTime);
            gameTimerProvider.ReadyTime
                .Where(t => 3 >= t && t > 0)
                .Subscribe(t =>
                {
                    seManager.PlayCountDown(); 
                }).AddTo(this);

            gameTimerProvider.ReadyTime
                .Where(t => t == 0)
                .Take(1)
                .Subscribe(_ =>
                {
                    bgmManager.FadeIn(2.0f);
                    seManager.PlayStart(); 
                    gameStateProvider.SetState(GameState.GamePlay);
                    gameTimerProvider.StartGame(GamePlayTime);
                }).AddTo(this);
            
            // Play
            gameTimerProvider.RemainingTime
                .Where(_ => gameStateProvider.IsGamePlay())
                .Subscribe(t =>
                {
                    switch (t)
                    {
                        case 30:
                            bgmManager.SetPitch(1.1f);
                            break;
                        case 20:
                            bgmManager.SetPitch(1.2f);
                            break;
                        case 10:
                            bgmManager.SetPitch(1.5f);
                            break;
                    }
                }).AddTo(this);
            
            gameTimerProvider.RemainingTime
                .Where(_ => gameStateProvider.IsGamePlay())
                .Select(t => t / 10)
                .Pairwise()
                .Where(p => p.Previous != p.Current)
                .Select(p => p.Current)
                .Subscribe(t =>
                {
                    timerView.StartCountDown(t, 10f);
                    seManager.PlayPig();
                }).AddTo(this);

            // ESCでリセット
            gameInputHandler.OnEscapePressed
                .Where(_ => gameStateProvider.IsGamePlay())
                .Subscribe(_ =>
                {
                    bgmManager.FadeOut(2.0f, 0.1f);
                    bgmManager.SetPitch(1.0f);
                    gameStateProvider.SetState(GameState.GameReady);
                    sceneTransitionManager.Run("Title", 1, false);
                }).AddTo(this);

            // Play -> Finish
            gameTimerProvider.RemainingTime
                .Where(_ => gameStateProvider.IsGamePlay())
                .Where(t => t == 0)
                .Subscribe(_ =>
                {
                    bgmManager.FadeOut(2.0f, 0.1f);
                    bgmManager.SetPitch(1.0f);
                    seManager.PlayFinish();
                    gameStateProvider.SetState(GameState.GameClear);
                }).AddTo(this);

            // Ranking -> Finish
            this.UpdateAsObservable()
                .Where(_ => gameStateProvider.IsGameClear())
                .Select(_ => SceneManager.sceneCount)
                .Select(_ => _ == 2)
                .Pairwise()
                .Where(_ => _.Previous && !_.Current)
                .Subscribe(_ =>
                {
                    gameStateProvider.SetState(GameState.GameFinish);
                    resultView.Show();
                }).AddTo(this);
        }
    }
}