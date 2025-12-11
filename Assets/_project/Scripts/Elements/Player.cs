using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameDirector gameDirector;

    public int startHealth;
    private int _currentHealth;

    public HealthBar healthBar;

    private PlayerMovement _playerMovement;

    public bool isDead;

    public GameObject shadow;

    public ParticleSystem collectedPS;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (transform.position.y < -10f && gameDirector.gameState == GameState.GamePlay)
        {
            gameDirector.LevelFailed(0);
        }
    }
    public void RestartPlayer()
    {
        transform.position = Vector3.zero;
        _currentHealth = startHealth;
        healthBar.SetFillBar(1);        
        _playerMovement.RestartPlayerMovement();
        isDead = false;
        EnableShadow();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Potion"))
        {
            other.gameObject.SetActive(false);
            _playerMovement.ChangeAnimationState("Win");
            gameDirector.fXManager.PlayPotionCollectPS(other.transform.position);
            gameDirector.LevelCompleted();
        }
    }

    public void GetHit(int damage)
    {
        if (isDead)
        {
            return;
        }
        _currentHealth -= damage;
        healthBar.SetFillBar((float)_currentHealth / startHealth);
        gameDirector.uIManager.getHitUI.ShowGetHitGradient();
        gameDirector.audioManager.PlayHitAS();
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        _playerMovement.ChangeAnimationState("Die");
        gameDirector.LevelFailed(2);
        DisableShadow();
    }

    public void DisableShadow()
    {
        shadow.SetActive(false);
    }
    public void EnableShadow()
    {
        shadow.SetActive(true);
    }

    public void PlayAlternativeFailAnimation()
    {
        _playerMovement.PlayAlternativeFailAnimation();
    }

    public void CollectableCollected()
    {
        collectedPS.Play();
        gameDirector.timerManager.CollectableCollected();
        gameDirector.audioManager.PlayCollectedAS();
    }

    public Vector3 GetCurrentDirection()
    {
        return _playerMovement.GetCurrentDirection();
    }
}
