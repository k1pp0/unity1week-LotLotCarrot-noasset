using Cysharp.Threading.Tasks;
using DG.Tweening;
using Suriyun;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

public class Player : ControllerPetZoo
{
    [SerializeField] private BoxCollider eatArea;
    [Inject] private SEManager seManager;

    public bool isEating;
    private int eatSpeed = 2000;
    private int eatingNumber = 0;

    public ReadOnlyReactiveProperty<int> TotalEatNumber => totalEatNumber.ToReadOnlyReactiveProperty();
    private readonly ReactiveProperty<int> totalEatNumber = new ReactiveProperty<int>(0);

    private void Start()
    {
        base.Start();

        this.OnTriggerEnterAsObservable()
            .Where(c => c.CompareTag("Eat"))
            .Subscribe(async c =>
            {
                eatingNumber++;
                await UniTask.WaitUntil(() => !isEating);
                c.GetComponent<Carrot>().Ate();
            }).AddTo(this);

        this.OnTriggerEnterAsObservable()
            .Where(_ => !isEating)
            .Where(c => c.CompareTag("Eat"))
            .Subscribe(async c =>
            {
                isEating = true;
                mecanim.SetBool(param_eating, true);
                SetDestination(transform.position);
                transform.DOLookAt(c.transform.position, 0.5f);

                eatArea.size *= 3.0f;
                await UniTask.Delay(eatSpeed);
                eatArea.size /= 3.0f;

                mecanim.SetBool(param_eating, false);
                transform.DOScale(transform.localScale + Vector3.one * 0.1f * eatingNumber, 1.0f)
                    .SetEase(Ease.OutBounce);
                seManager.PlayEat();
                totalEatNumber.Value += eatingNumber;
                eatingNumber = 0;
                isEating = false;
            }).AddTo(this);
    }

    public void EatFinish()
    {
        eatArea.enabled = false;
    }
}