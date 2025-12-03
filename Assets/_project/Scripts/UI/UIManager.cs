using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public MainMenu mainMenu;
    public WinUI winUI;
    public FailUI failUI;
    public void ShowMainMenu()
    {
        mainMenu.Show();
        winUI.Hide();
        failUI.Hide();
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
