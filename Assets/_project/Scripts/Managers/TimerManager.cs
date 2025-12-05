using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public TimerUI timerUI;
    public Player player;
    private float _remainingTime;
    private float _totalTime;

    public void RestartTimerManager(float levelTime)
    {
        _remainingTime = levelTime;
        _totalTime = levelTime;
    }

    private void Update()
    {
        if (gameDirector.gameState != GameState.GamePlay)
        {
            return;
        }
        _remainingTime -= Time.deltaTime;
        timerUI.SetFillBar(_remainingTime, _totalTime);
        if (_remainingTime <= 0)
        {
            player.PlayAlternativeFailAnimation();
            gameDirector.LevelFailed(3);
        }
    }
}
