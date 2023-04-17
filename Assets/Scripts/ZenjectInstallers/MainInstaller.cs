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
    }
}
