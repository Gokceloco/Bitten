using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameDirector gameDirector;
    public List<Level> levelPrefabs;
    public int currentLevelNo;
    private Level _currentLevel;
    public void RestartLevelManager()
    {
        DeleteCurrentLevel();
        CreateNewLevel();
    }

    public void StopLevel()
    {
        _currentLevel.StopLevel();
    }

    public float GetCurrentLevelTime()
    {
        return _currentLevel.levelTime;
    }

    private void CreateNewLevel()
    {
        _currentLevel = Instantiate(levelPrefabs[currentLevelNo - 1]);
        _currentLevel.transform.position = Vector3.zero;
        _currentLevel.StartLevel(this);
    }

    private void DeleteCurrentLevel()
    {
        if (_currentLevel)
        {
            Destroy(_currentLevel.gameObject);
        }
    }
}
