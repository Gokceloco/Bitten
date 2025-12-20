using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Enemy> enemyPrefabs;

    private LevelManager _levelManager;

    public float spawnRange;
    public void StartEnemySpawner(LevelManager levelManager)
    {
        _levelManager = levelManager;
        var state = Random.state;
        Random.InitState(_levelManager.currentLevelNo);
        
        SpawnEnemies();

        Random.state = state;
    }

    private void SpawnEnemies()
    {
        int enemyCount = (_levelManager.currentLevelNo - 1) / _levelManager.levelPrefabs.Count;

        for (int i = 0; i < enemyCount; i++)
        {
            var newEnemy = Instantiate(
                enemyPrefabs[Random.Range(0,enemyPrefabs.Count)],
                transform.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange)),
                Quaternion.identity
                );
            newEnemy.transform.SetParent(GetComponentInParent<Level>().transform);
            newEnemy.StartEnemy(_levelManager.gameDirector.player);
        }
    }
}
