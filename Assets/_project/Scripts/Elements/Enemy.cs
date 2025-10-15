using Mono.Cecil.Cil;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;

    public int startHealth;
    private int _currentHealth;

    public float speed;
    public float playerWalkTowardsDistance;

    public ActionState actionState;

    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private Player _player;

    public LayerMask playerSeeLayerMask;

    private Vector3 _playerLastSeenPosition;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
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
            if (GetIfEnemySeesPlayer())
            {
                actionState = ActionState.WalkTowardsPlayer;
            }
            else if (_playerLastSeenPosition != Vector3.zero)
            {
                actionState = ActionState.WalkTowardsPlayerLastSeenPos;                
            }
        }

        //Action States
        if (actionState == ActionState.WalkTowardsPlayer)
        {
            WalkTowardsPlayer();
        }
        else if (actionState == ActionState.WalkTowardsPlayerLastSeenPos)
        {
            WalkTowardsPlayerLastPosition();
        }
        else if (actionState == ActionState.Standing)
        {
            StopEnemy();
        }
    }

    

    private bool GetIfEnemySeesPlayer()
    {
        if (Physics.Raycast(transform.position + Vector3.up,
            _player.transform.position - transform.position,
            playerWalkTowardsDistance, playerSeeLayerMask))
        {
            return false;
        }
        _playerLastSeenPosition = _player.transform.position;
        return true;
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
        _navMeshAgent.SetDestination(_player.transform.position);
    }
    private void WalkTowardsPlayerLastPosition()
    {
        _navMeshAgent.SetDestination(_playerLastSeenPosition);
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
    WalkTowardsPlayerLastSeenPos,
    Attack,
    Dead,
}