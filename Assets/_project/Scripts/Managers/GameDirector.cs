using System;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public LevelManager levelManager;
    public AudioManager audioManager;
    public FXManager fXManager;
    public TimerManager timerManager;
    public Player player;

    public UIManager uIManager;

    public GameState gameState;

    private void Start()
    {        
        uIManager.ShowMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RestartLevel();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            LoadPreviousLevel();
        }
    }

    private void LoadPreviousLevel()
    {
        levelManager.currentLevelNo--;
        if (levelManager.currentLevelNo < 1)
        {
            levelManager.currentLevelNo = 1;
        }
        RestartLevel();
    }

    public void LoadNextLevel()
    {
        levelManager.currentLevelNo++;
        if (levelManager.currentLevelNo >= levelManager.levelPrefabs.Count)
        {
            levelManager.currentLevelNo = levelManager.levelPrefabs.Count;
        }
        RestartLevel();
    }

    public void RestartLevel()
    {
        gameState = GameState.GamePlay;
        levelManager.RestartLevelManager();
        player.RestartPlayer();
        audioManager.PlayAmbientSound();
        timerManager.RestartTimerManager(levelManager.GetCurrentLevelTime());
        uIManager.ShowInGameUI();
    }

    public void LevelCompleted()
    {
        gameState = GameState.WinUI;
        audioManager.PlayVictoryAS();
        audioManager.StopAmbientSound();
        uIManager.ShowWinUI(3);
        uIManager.HideInGameUI();
    }

    public void LevelFailed(float delay)
    {
        levelManager.StopLevel();
        gameState = GameState.LoseUI;
        uIManager.ShowFailUI(delay);
        audioManager.PlayFailAS();
        audioManager.StopAmbientSound();
        uIManager.HideInGameUI();
    }
}
public enum GameState
{
    MainMenu,
    GamePlay,
    WinUI,
    LoseUI,
    Inventory,
    EscapeMenu,
}