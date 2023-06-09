using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISaveManager>().To<SaveManager>().AsSingle();
        Container.Bind<ISession>().To<Session>().AsSingle();
        Container.Bind<ISceneManager>().To<EngineSceneManager>().AsSingle();
        Container.Bind<ICommandProcessor>().To<CommandProcessor>().AsSingle();
        Container.Bind<IInputManager>().To<InputManager>().AsSingle();
        Container.Bind<IPauseManager>().To<PauseManager>().AsSingle();
        Container.Bind<IEngine>().To<Engine>().AsSingle();
        Container.Bind<IUIManager>().To<UIManager>().AsSingle();
        Container.Bind<IWorldManager>().To<WorldManager>().AsSingle();
    }
}
