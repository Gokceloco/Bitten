using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    public float levelTime;
    private LevelManager _levelManager;

    private List<Enemy> _enemies = new List<Enemy>();
    public void StartLevel(LevelManager levelManager)
    {
        _enemies = GetComponentsInChildren<Enemy>().ToList();
        _levelManager = levelManager;
        foreach (var e in _enemies)
        {
            e.StartEnemy(_levelManager.gameDirector.player);
        }
    }

    public bool IsLevelCleared()
    {
        if (_enemies.Count == 0)
        {
            return true;
        }
        return false;
    }

    public void StopLevel()
    {
        foreach (var e in _enemies)
        {
            e.SetPlayerDead();
        }
    }

    public void EnemyDestroyed(Enemy enemy)
    {
        _enemies.Remove(enemy);
    }

    public void ShowClearLevelMsg()
    {
        _levelManager.gameDirector.uIManager.messageUI.ShowMessage("CLEAR THE LEVEL!", 3, 0);
    }

    public int GetCoinUpgradeCount()
    {
        return _levelManager.gameDirector.upgradeManager.lootUpgradeCount; 
    }
}
