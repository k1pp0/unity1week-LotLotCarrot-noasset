using GameJamUtility;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameTimerProvider>().AsSingle();
    }
}