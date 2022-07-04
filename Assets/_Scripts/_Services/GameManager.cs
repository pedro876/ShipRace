using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// GameManager utilizes lazy initialization (static constructor), so services won't be installed until needed.
/// </summary>
public static class GameManager
{
    public static readonly IServiceLocator serviceLocator = new ServiceLocator();

    static GameManager()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        serviceLocator.RegisterService<IGyroSystem>(new NullGyroSystem());
        Debug.Log("GameManager initialized");
    }
}