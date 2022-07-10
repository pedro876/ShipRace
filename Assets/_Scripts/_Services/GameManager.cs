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
        Settings,
        Pause,
        GameOver
    }

    public GameState currentState { get; private set; }
    private bool stateBlocked = false;
    private Player player;
    private PlayerInputAdapter playerInput;
    private UIInputAdapter uiInput;
    private LevelManager level;
    private bool musicOn = true;
    private bool sfxOn = true;

    public event Action<GameState> onStateChanged;

    public void SetState(GameState newState)
    {
        if (stateBlocked) return;
        bool reset = currentState == GameState.Pause && newState != GameState.Game;
        reset = reset || (currentState == GameState.GameOver && newState != GameState.Game);
        bool resume = currentState == GameState.Pause && newState == GameState.Game;
        if (newState == currentState) return;
        currentState = newState;
        onStateChanged?.Invoke(newState);
        if(reset)
        {
            Debug.Log("Reseting game");
            level.ResetLevel();
            player.ResetPlayer();
        }

        if (newState == GameState.Pause)
        {
            PauseGame();
        }
        else if (resume)
        {
            ResumeGame();
        }

        Debug.Log($"new state: {newState}");
    }

    public void BlockState()
    {
        stateBlocked = true;
    }

    public void ReleaseState()
    {
        stateBlocked = false;
    }

    public bool IsMusicOn()
    {
        return musicOn;
    }

    public bool IsSfxOn()
    {
        return sfxOn;
    }

    public int GetCurrentScore()
    {
        return player.GetScore();
    }

    public void SetMusic(bool musicOn)
    {
        Debug.Log("Music turned: " + musicOn);
        this.musicOn = musicOn;
    }

    public void SetSFX(bool sfxOn)
    {
        Debug.Log("Sfx turned: " + sfxOn);
        this.sfxOn = sfxOn;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void PauseGame()
    {
        player.BlockInput();
        player.BlockMotion();
        level.Pause();
    }

    private void ResumeGame()
    {
        player.ReleaseInput();
        player.ReleaseMotion();
        level.Resume();
    }

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        playerInput = FindObjectOfType<PlayerInputAdapter>();
        uiInput = FindObjectOfType<UIInputAdapter>();
        serviceLocator.RegisterService<UIInputAdapter>(uiInput);
        player = FindObjectOfType<Player>();
        player.SetInput(playerInput);
        level = FindObjectOfType<LevelManager>();
        instance = this;
        SetState(GameState.Init);
    }

    private void Start()
    {
        level.SetPlayer(player.GetShipTransform());
    }

    
}