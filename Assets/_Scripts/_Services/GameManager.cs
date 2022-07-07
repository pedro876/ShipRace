using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// GameManager utilizes lazy initialization (static constructor), so services won't be installed until needed.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region static

    public static readonly IServiceLocator serviceLocator = new ServiceLocator();

    public static GameManager instance;

    static GameManager()
    {
        //INSTALL NULL SERVICES
        serviceLocator.RegisterService<IGyroSystem>(new NullGyroSystem());
        Debug.Log("GameManager initialized");
    }

    #endregion

    public enum GameState
    {
        Init,
        Menu,
        CountDown,
        Game,
        Exit,
        Options
    }

    public GameState currentState { get; private set; }
    private Player player;
    private LevelManager level;

    public event Action<GameState> onStateChanged;
    /*public void SetInitState() => SetState(GameState.Init);
    public void SetMenuState() => SetState(GameState.Menu);
    public void SetCountDownState() => SetState(GameState.CountDown);
    public void SetGameState() => SetState(GameState.Game);*/

    public void SetState(GameState newState)
    {
        if (newState == currentState) return;
        currentState = newState;
        onStateChanged?.Invoke(newState);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        player = FindObjectOfType<Player>();
        level = FindObjectOfType<LevelManager>();
        instance = this;
        SetState(GameState.Init);
    }

    private void Start()
    {
        level.SetPlayer(player.GetShipTransform());
    }
}