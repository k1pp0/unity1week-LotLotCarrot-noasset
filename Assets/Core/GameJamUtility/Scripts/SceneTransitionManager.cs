using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameJamUtility
{
    public class SceneTransitionManager : ISceneTransitionManager
    {
        [Inject] private TransitionStyleManager transitionStyleManager;

        public async void Run(string name, int index, bool isContainClose)
        {
            await transitionStyleManager.Play(index, isContainClose);
            await SceneManager.LoadSceneAsync(name);
            transitionStyleManager.Close(index);
        }
    }
}