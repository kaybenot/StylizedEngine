using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Platform : Singleton<Platform>
{
    [Inject] private ISession session;

    private void Start()
    {
        InitializeEngine();
    }

    private void InitializeEngine()
    {
        session.New();
        session.Initialize();
    }
}
