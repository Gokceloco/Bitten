using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

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

    public VideoPlayer videoPlayer;

    private Coroutine _videoPlayCoroutine;

    private void Update()
    {
        if (gameDirector.gameState == GameState.GamePlay && Input.GetKeyDown(KeyCode.Escape))
        {
            ShowEscapeMenu();
        }
        else if (gameDirector.gameState == GameState.Video && Input.GetKeyDown(KeyCode.Escape))
        {
            FinalizeVideo();
            if (_videoPlayCoroutine != null) 
            { 
                StopCoroutine(_videoPlayCoroutine);
            }
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
    }
    public void ShowFailUI(float delay)
    {
        failUI.Show(delay);
    }
    public void ShowInGameUI()
    {
        timerUI.Show();
        getHitUI.Show();
        messageUI.Show();
        
        if (gameDirector.levelManager.currentLevelNo > 11)
        {
            inventoryUI.Show();
        }
        else
        {
            inventoryUI.Hide();
        }
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
        if (gameDirector.levelManager.currentLevelNo == 1 && !gameDirector.webMode)
        {
            _videoPlayCoroutine = StartCoroutine(VideoPlayCoroutine());
        }
        else
        {
            mainMenu.Hide();
            gameDirector.RestartLevel();
        }
    }

    IEnumerator VideoPlayCoroutine()
    {
        gameDirector.gameState = GameState.Video;
        videoPlayer.enabled = true;
        videoPlayer.Play();
        yield return new WaitForSeconds(9);        
        FinalizeVideo();
    }

    void FinalizeVideo()
    {
        videoPlayer.enabled = false;
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
