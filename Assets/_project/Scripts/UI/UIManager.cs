using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public MainMenu mainMenu;
    public WinUI winUI;
    public FailUI failUI;
    public EscapeMenu escapeMenu;
    public TimerUI timerUI;
    public GetHitUI getHitUI;
    public MessageUI messageUI;
    public InventoryUI inventoryUI;
    public GrenadeUI grenadeUI;
    public CoinUI coinUI;
    public UpgradeUI upgradeUI;
    public UnlockUI unlockUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowEscapeMenu();
        }
    }
    public void ShowMainMenu()
    {
        gameDirector.gameState = GameState.MainMenu;
        mainMenu.Show();
        winUI.Hide();
        failUI.Hide();
        escapeMenu.Hide();
        HideInGameUI();
        upgradeUI.Hide();
        coinUI.Show();
        unlockUI.Hide();
    }
    public void ShowEscapeMenu()
    {
        Time.timeScale = 0;
        escapeMenu.Show(0);
        gameDirector.gameState = GameState.EscapeMenu;
    }
    public void ShowWinUI(float delay)
    {
        winUI.Show(delay);
        upgradeUI.Show(delay);   
    }
    public void ShowFailUI(float delay)
    {
        failUI.Show(delay);
        upgradeUI.Show(delay);
    }
    public void ShowInGameUI()
    {
        timerUI.Show();
        getHitUI.Show();
        messageUI.Show();
        inventoryUI.Show();
        if (gameDirector.levelManager.currentLevelNo > 5)
        {
            grenadeUI.Show();
        }
        else
        {
            grenadeUI.Hide();
        }
    }
    public void HideInGameUI()
    {
        timerUI.Hide();
        getHitUI.Hide();
        messageUI.Hide();
        inventoryUI.Hide();
        grenadeUI.Hide();        
    }

    //Callback Functions
    public void ResumeButtonPressed()
    {
        Time.timeScale = 1;
        escapeMenu.Hide();
        Invoke(nameof(ChangeGameStateToGamePlay), .1f);
    }
    void ChangeGameStateToGamePlay()
    {
        gameDirector.gameState = GameState.GamePlay;
    }
    public void MainMenuButtonPressed()
    {
        Time.timeScale = 1;
        ShowMainMenu();
    }

    public void UpgradeButtonPressed()
    {
        upgradeUI.Show(0);
    }
    public void ExitButtonPressed()
    {
        Application.Quit();
    }
    public void StartGameButtonPressed()
    {
        mainMenu.Hide();
        gameDirector.RestartLevel();
    }
    public void LoadNextLevelButtonPressed()
    {
        upgradeUI.Hide();
        winUI.Hide();
        gameDirector.LoadNextLevel();
    }
    public void RetryButtonPressed()
    {
        upgradeUI.Hide();
        failUI.Hide();
        gameDirector.RestartLevel();
    }
}
