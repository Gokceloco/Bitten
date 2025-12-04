using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public MainMenu mainMenu;
    public WinUI winUI;
    public FailUI failUI;
    public EscapeMenu escapeMenu;

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
    }

    public void ShowEscapeMenu()
    {
        Time.timeScale = 0;
        escapeMenu.Show(0);
        gameDirector.gameState = GameState.EscapeMenu;
    }

    public void ResumeButtonPressed()
    {
        Time.timeScale = 1;
        escapeMenu.Hide();
        gameDirector.gameState = GameState.GamePlay;
    }
    public void MainMenuButtonPressed()
    {
        Time.timeScale = 1;
        ShowMainMenu();
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
        winUI.Hide();
        gameDirector.LoadNextLevel();
    }
    public void RetryButtonPressed()
    {
        failUI.Hide();
        gameDirector.RestartLevel();
    }

    public void ShowWinUI(float delay)
    {
        winUI.Show(delay);
    }
    public void ShowFailUI(float delay)
    {
        failUI.Show(delay);
    }
}
