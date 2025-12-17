using System;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public LevelManager levelManager;
    public AudioManager audioManager;
    public FXManager fXManager;
    public TimerManager timerManager;
    public CoinManager coinManager;
    public UpgradeManager upgradeManager;
    public Player player;


    public UIManager uIManager;

    public GameState gameState;

    private void Start()
    {
        LoadPersistanceData();
        uIManager.ShowMainMenu();
    }

    private void LoadPersistanceData()
    {
        var level = PlayerPrefs.GetInt("LastLevel");
        level = Mathf.Max(level, 1);
        levelManager.SetStartingLevel(level);
        coinManager.SetStartingCoinCount(PlayerPrefs.GetInt("CoinCount"));
        upgradeManager.SetStartingUpgrades(
            PlayerPrefs.GetInt("AttackUpgrades"),
            PlayerPrefs.GetInt("DefenceUpgrades"),
            PlayerPrefs.GetInt("CoinUpgrades"));
    }

    private void ResetPersistanceData()
    {
        PlayerPrefs.SetInt("LastLevel", 1);
        PlayerPrefs.SetInt("CoinCount", 0);
        PlayerPrefs.SetInt("AttackUpgrades", 0);
        PlayerPrefs.SetInt("DefenceUpgrades", 0);
        PlayerPrefs.SetInt("CoinUpgrades", 0);
        LoadPersistanceData();
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
        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetPersistanceData();
            RestartLevel();
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
        Invoke(nameof(ChangeGameStateToGamePlay), .1f);
        levelManager.RestartLevelManager();
        player.RestartPlayer();
        audioManager.PlayAmbientSound();
        timerManager.RestartTimerManager(levelManager.GetCurrentLevelTime());
        uIManager.ShowInGameUI();
        if (levelManager.currentLevelNo == 1)
        {
            uIManager.messageUI.ShowMessage("WASD TO MOVE AROUND!", 3, 0);
            uIManager.messageUI.ShowMessage("FIND THE POTION BEFORE TIME RUNS OUT!", 3, 4);
        }
    }

    void ChangeGameStateToGamePlay()
    {
        gameState = GameState.GamePlay;
    }

    public void LevelCompleted()
    {
        gameState = GameState.WinUI;
        audioManager.PlayVictoryAS();
        audioManager.StopAmbientSound();
        uIManager.ShowWinUI(3);
        uIManager.HideInGameUI();
        PlayerPrefs.SetInt("LastLevel", levelManager.currentLevelNo + 1);
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