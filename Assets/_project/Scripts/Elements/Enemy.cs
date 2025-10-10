using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;

    public int startHealth;
    private int _currentHealth;

    public void StartEnemy()
    {
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
    }
    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        print(_currentHealth);
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
