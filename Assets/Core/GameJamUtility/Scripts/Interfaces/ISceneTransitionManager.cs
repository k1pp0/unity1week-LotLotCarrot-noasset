namespace GameJamUtility
{
    public interface ISceneTransitionManager
    {
        void Run(string name, int index, bool isContainClose);
    }
}