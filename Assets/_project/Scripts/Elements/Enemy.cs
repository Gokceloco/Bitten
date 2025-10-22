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
    private AnimationState _animationStateBeforeGetHit;

    private Rigidbody _rb;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Player _player;

    public LayerMask playerSeeLayerMask;

    private Vector3 _playerLastSeenPosition;

    private bool _isAttackInProgress;
    private CapsuleCollider _capsuleCollider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
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
            SwitchAnimation(AnimationState.Attack, true);
            StartCoroutine(AttackCoroutine(1.2f));
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
        if (currentAnimationState != AnimationState.GetHit)
        {
            _navMeshAgent.SetDestination(_player.transform.position);
            _navMeshAgent.isStopped = false;
            SwitchAnimation(AnimationState.Walk);
        }        
    }
    private void WalkTowardsPlayerLastPosition()
    {

        if (currentAnimationState != AnimationState.GetHit)
        {
            _navMeshAgent.SetDestination(_playerLastSeenPosition);
            _navMeshAgent.isStopped = false;
            SwitchAnimation(AnimationState.Walk);
        }
    }

    private void SwitchAnimation(AnimationState desiredAnimationState, bool forcePlayAnimation = false)
    {
        if (desiredAnimationState == AnimationState.Walk && (currentAnimationState != AnimationState.Walk || forcePlayAnimation))
        {
            _animator.SetTrigger("Walk");
            currentAnimationState = AnimationState.Walk;
        }
        else if (desiredAnimationState == AnimationState.Idle && (currentAnimationState != AnimationState.Idle || forcePlayAnimation))
        {
            _animator.SetTrigger("Idle");
            currentAnimationState = AnimationState.Idle;
        }
        else if (desiredAnimationState == AnimationState.Attack && (currentAnimationState != AnimationState.Attack || forcePlayAnimation))
        {
            _animator.SetTrigger("Attack");
            currentAnimationState = AnimationState.Attack;
        }
        else if (desiredAnimationState == AnimationState.GetHit && (currentAnimationState != AnimationState.GetHit || forcePlayAnimation))
        {
            _animator.SetTrigger("GetHit");
            currentAnimationState = AnimationState.GetHit;
        }
        else if (desiredAnimationState == AnimationState.Die && (currentAnimationState != AnimationState.Die || forcePlayAnimation))
        {
            _animator.SetTrigger("Die");
            currentAnimationState = AnimationState.Die;
        }
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        StartCoroutine(PlayGetHitCoroutine());
        healthBar.SetHealthBar((float)_currentHealth / startHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator PlayGetHitCoroutine()
    {
        if (currentAnimationState != AnimationState.GetHit)
        {
            _animationStateBeforeGetHit = currentAnimationState;
        }
        _navMeshAgent.isStopped = true;
        SwitchAnimation(AnimationState.GetHit);
        yield return new WaitForSeconds(.1f);
        SwitchAnimation(_animationStateBeforeGetHit);
    }

    private void Die()
    {
        actionState = ActionState.Dead;
        _animationStateBeforeGetHit = AnimationState.Die;
        _navMeshAgent.isStopped = true;
        _capsuleCollider.enabled = false;
        SwitchAnimation(AnimationState.Die);
        Destroy(gameObject, 3);
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
    GetHit,
    Die,
}