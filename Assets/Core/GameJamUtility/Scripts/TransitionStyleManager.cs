using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameJamUtility
{
    public class TransitionStyleManager : MonoBehaviour
    {
        [Header("CANVAS")]
        [SerializeField] private Canvas canvas;
        
        [Header("STYLE PARENTS")]
        [SerializeField] private TransitionStyle[] styles;

        [Header("LOADING")]
        [SerializeField] private CanvasGroup loading;
        private float _alpha;
        private const float TransitionTime = 1.0f;

        public async UniTask Play(int index, bool isContainClose)
        {
            canvas.sortingOrder = 1;
            // StartCoroutine(ShowLoading());
            await styles[index].Play(isContainClose);
        }
        public void Close(int index)
        {
            // StartCoroutine(HideLoading());
            styles[index].Close();
        }

        private IEnumerator ShowLoading()
        {
            var t = 0f;
            while (t < TransitionTime)
            {
                t += Time.deltaTime;
                var rate = t / TransitionTime;
                loading.alpha = Mathf.Clamp01(rate);
                yield return null;
            }
        }

        private IEnumerator HideLoading()
        {
            var t = TransitionTime * 0.5f;
            while (t > 0)
            {
                t -= Time.deltaTime;
                var rate = t / TransitionTime * 0.5f;
                loading.alpha = Mathf.Clamp01(rate);
                yield return null;
            }
        }
    }
}