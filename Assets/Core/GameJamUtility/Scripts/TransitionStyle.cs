using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace GameJamUtility
{
    public class TransitionStyle : MonoBehaviour
    {
        private const string PanelFadeIn = "Panel Open";
        private const string PanelFadeOut = "Panel Close";
        private const string StyleExpand = "Expand";
        private const string StyleClose = "Close";

        [Header("PANEL ANIMATOR")] [SerializeField]
        private Animator panel;

        [Header("STYLE ANIMATOR")] [SerializeField]
        private Animator style;

        public UniTask Play(bool isContainClose)
        {
            panel.Play(PanelFadeIn, 0, 0);
            style.Play(StyleExpand, 0, 0);

            if (isContainClose)
            {
                return style.ObserveEveryValueChanged(_ => style.GetCurrentAnimatorStateInfo(0))
                    .Skip(1)
                    .Select(state => state.IsName(StyleExpand))
                    .Pairwise()
                    .Where(pair => pair.Previous && !pair.Current)
                    .Take(1)
                    .Do(_ => style.speed = 0.1f)
                    .ToUniTask();
            }
            else
            {
                return style.ObserveEveryValueChanged(_ => style.GetCurrentAnimatorStateInfo(0))
                    .Skip(1)
                    .Select(state => state.normalizedTime)
                    .Where(t => t > 0.5f)
                    .Take(1)
                    .Do(_ => style.speed = 0.1f)
                    .ToUniTask();
            }
        }

        public void Close()
        {
            style.speed = 1.0f;
        }
    }
}