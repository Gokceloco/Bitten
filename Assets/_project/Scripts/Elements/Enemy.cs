using DG.Tweening;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;

    public int startHealth;
    private int _currentHealth;

    public float speed;
    public float playerWalkTowardsDistance;
    public float playerAttackDistance;

    public ActionState actionState;
    public AnimationState currentAnimationState;

    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Player _player;

    public LayerMask playerSeeLayerMask;

    private Vector3 _playerLastSeenPosition;

    private bool _isAttackInProgress;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
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
        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            actionState = ActionState.Attack;
        }
        else if (GetDistanceFromPlayer() < playerWalkTowardsDistance && !_isAttackInProgress)
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
        else if (actionState == ActionState.Attack)
        {
            AttackPlayer();
        }
        else if (actionState == ActionState.Standing)
        {
            StopEnemy();
        }
    }

    private void AttackPlayer()
    {
        if (!_isAttackInProgress)
        {
            _isAttackInProgress = true;
            _navMeshAgent.isStopped = true;
            SwitchAnimation(AnimationState.Idle);
            StartCoroutine(AttackCoroutine(2));
        }
    }

    IEnumerator AttackCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            _player.GetHit(1);
        }
        _isAttackInProgress = false;
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
        SwitchAnimation(AnimationState.Idle);
    }

    private float GetDistanceFromPlayer()
    {
        return (transform.position - _player.transform.position).magnitude;
    }

    private void WalkTowardsPlayer()
    {
        _navMeshAgent.SetDestination(_player.transform.position);
        _navMeshAgent.isStopped = false;
        SwitchAnimation(AnimationState.Walk);
    }
    private void WalkTowardsPlayerLastPosition()
    {
        _navMeshAgent.SetDestination(_playerLastSeenPosition);
        _navMeshAgent.isStopped = false;
        SwitchAnimation(AnimationState.Walk);
    }

    private void SwitchAnimation(AnimationState desiredAnimationState)
    {
        if (desiredAnimationState == AnimationState.Walk && currentAnimationState != AnimationState.Walk)
        {
            _animator.SetTrigger("Walk");
            currentAnimationState = AnimationState.Walk;
        }
        else if (desiredAnimationState == AnimationState.Idle && currentAnimationState != AnimationState.Idle)
        {
            _animator.SetTrigger("Idle");
            currentAnimationState = AnimationState.Idle;
        }
        else if (desiredAnimationState == AnimationState.Attack && currentAnimationState != AnimationState.Attack)
        {
            _animator.SetTrigger("Attack");
            currentAnimationState = AnimationState.Attack;
        }
        else if (desiredAnimationState == AnimationState.Die && currentAnimationState != AnimationState.Die)
        {
            _animator.SetTrigger("Die");
            currentAnimationState = AnimationState.Die;
        }
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
public enum AnimationState
{
    Idle,
    Walk,
    Attack,
    Die,
}