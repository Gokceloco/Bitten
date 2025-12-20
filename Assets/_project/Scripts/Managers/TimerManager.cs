using System;
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
        _remainingTime = levelTime + ((gameDirector.levelManager.currentLevelNo - 1) 
            / gameDirector.levelManager.levelPrefabs.Count)*2;
        _totalTime = _remainingTime;
    }

    public void CollectableCollected()
    {
        _remainingTime += 1f;
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
