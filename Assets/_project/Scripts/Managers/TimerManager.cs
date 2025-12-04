using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public Player player;
    private float _remainingTime;

    public void RestartTimerManager(float levelTime)
    {
        _remainingTime = levelTime;
    }

    private void Update()
    {
        if (gameDirector.gameState != GameState.GamePlay)
        {
            return;
        }
        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0)
        {
            player.PlayAlternativeFailAnimation();
            gameDirector.LevelFailed(3);
        }
        print(_remainingTime);
    }
}
