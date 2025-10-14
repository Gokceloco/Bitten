using Mono.Cecil.Cil;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;

    public int startHealth;
    private int _currentHealth;

    public float speed;
    public float playerWalkTowardsDistance;

    public ActionState actionState;

    private Rigidbody _rb;
    private Player _player;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void StartEnemy(Player player)
    {
        _currentHealth = startHealth;
        healthBar.SetHealthBar(1);
        _player = player;
    }

    private void Update()
    {
        if (actionState == ActionState.Dead)
        {
            return;
        }

        //Decider Logic
        if (GetDistanceFromPlayer() < playerWalkTowardsDistance)
        {
            actionState = ActionState.WalkTowardsPlayer;
        }
        else
        {
            actionState = ActionState.Standing;
        }

        //Action States
        if (actionState == ActionState.WalkTowardsPlayer)
        {
            WalkTowardsPlayer();
        }
        else if (actionState == ActionState.Standing)
        {
            StopEnemy();
        }
    }

    private void StopEnemy()
    {
        _rb.linearVelocity = Vector3.zero;
    }

    private float GetDistanceFromPlayer()
    {
        return (transform.position - _player.transform.position).magnitude;
    }

    private void WalkTowardsPlayer()
    {
        var dir = Vector3.zero;
        dir = (_player.transform.position - transform.position).normalized;
        _rb.linearVelocity = dir * speed;
    }

    
    public void GetHit(int damage)
    {
        _currentHealth -= damage;
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

public enum ActionState
{
    Standing,
    WalkTowardsPlayer,
    Attack,
    Dead,
}