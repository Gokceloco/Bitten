using DG.Tweening;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public HealthBar healthBar;

    public int startHealth;
    private int _currentHealth;

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

    public List<Light> eyeLights;

    private Coroutine _attackCoroutine;

    public GameObject shadow;
    public Light mainLight;

    private HitFlash _hitFlash;

    public Transform chestBone;

    private bool _didSeePlayer;

    public EnemyType enemyType;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _hitFlash = GetComponent<HitFlash>();
    }

    public void StartEnemy(Player player)
    {
        _currentHealth = startHealth;
        healthBar.SetFillBar(1);
        _player = player;
    }

    private void Update()
    {
        if (actionState == ActionState.Dead || _player.gameDirector.gameState != GameState.GamePlay)
        {
            return;
        }

        //Decider Logic
        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            actionState = ActionState.Attack;
        }
        else if ((GetDistanceFromPlayer() < playerWalkTowardsDistance || _didSeePlayer) && !_isAttackInProgress)
        {
            if (GetIfEnemySeesPlayer() || _didSeePlayer)
            {
                actionState = ActionState.WalkTowardsPlayer;
            }
            /*else if (_playerLastSeenPosition != Vector3.zero)
            {
                if ((transform.position - _playerLastSeenPosition).magnitude < 1f)
                {
                    actionState = ActionState.Standing;
                }
                else
                {
                    actionState = ActionState.WalkTowardsPlayerLastSeenPos;
                }
            }*/
        }

        //Action States
        if (actionState == ActionState.WalkTowardsPlayer)
        {
            WalkTowardsPlayer();
            if (!_didSeePlayer)
            {
                _didSeePlayer = true;
                _player.gameDirector.audioManager.PlayZombieScreamAS();
            }
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
            SwitchAnimation(AnimationState.Idle);
            _navMeshAgent.isStopped = true;
        }
    }

    private void AttackPlayer()
    {
        if (!_isAttackInProgress)
        {
            _isAttackInProgress = true;
            _navMeshAgent.isStopped = true;
            SwitchAnimation(AnimationState.Attack, true);
            _attackCoroutine = StartCoroutine(AttackCoroutine(1.2f));
        }
    }

    IEnumerator AttackCoroutine(float hitDelay)
    {
        yield return new WaitForSeconds(hitDelay);
        if (GetDistanceFromPlayer() < playerAttackDistance)
        {
            var damage = 1;
            if (enemyType == EnemyType.Tough)
            {
                damage = 3;
            }
            _player.GetHit(damage);
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

    public void SetPlayerDead()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
        _navMeshAgent.isStopped = true;
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
            _animator.ResetTrigger("Idle");
            _animator.CrossFade("Walk", .1f);
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
            StartCoroutine(UpperBodyMaskCoroutine(.4f, .5f));
        }
        else if (desiredAnimationState == AnimationState.Die && (currentAnimationState != AnimationState.Die || forcePlayAnimation))
        {
            _animator.CrossFade("Die", .1f);
            currentAnimationState = AnimationState.Die;
        }
    }

    IEnumerator UpperBodyMaskCoroutine(float delay, float amount)
    {
        _animator.SetLayerWeight(1, amount);
        yield return new WaitForSeconds(delay);
        _animator.SetLayerWeight(1, 0);
    }

    public void GetHit(int damage)
    {
        _currentHealth -= damage;
        StartCoroutine(PlayGetHitCoroutine());
        healthBar.SetFillBar((float)_currentHealth / startHealth);
        _player.gameDirector.fXManager.SpawnFloatingText(damage, transform.position);
        _hitFlash.PlayHitFlash();
        _player.gameDirector.audioManager.PlayHitAS();
        if (actionState == ActionState.Standing)
        {
            actionState = ActionState.WalkTowardsPlayer;
        }
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator PlayGetHitCoroutine()
    {
        CancelAttack();
        if (currentAnimationState != AnimationState.GetHit)
        {
            _animationStateBeforeGetHit = currentAnimationState;
        }
        _navMeshAgent.isStopped = true;
        SwitchAnimation(AnimationState.GetHit);
        yield return new WaitForSeconds(.3f);
        SwitchAnimation(_animationStateBeforeGetHit);
    }

    private void Die()
    {
        CancelAttack();
        actionState = ActionState.Dead;
        _animationStateBeforeGetHit = AnimationState.Die;
        _navMeshAgent.isStopped = true;
        _capsuleCollider.enabled = false;
        SwitchAnimation(AnimationState.Die);
        foreach (var e in eyeLights)
        {
            e.enabled = false;
        }
        shadow.SetActive(false);
        mainLight.enabled = false;      
        _player.gameDirector.fXManager.PlayZombieDestroyPSDelayed(2.7f, chestBone);
        GetComponentInParent<Level>().EnemyDestroyed(this);

        var minCollectableCount = 1;
        var maxCollectableCount = 4;

        if (enemyType == EnemyType.Tough)
        {
            minCollectableCount = 3;
            maxCollectableCount = 8;
        }

        int bonusCoinCount = GetComponentInParent<Level>().GetCoinUpgradeCount();

        minCollectableCount += bonusCoinCount;
        maxCollectableCount += bonusCoinCount;

        for (int i = 0; i < Random.Range(minCollectableCount, maxCollectableCount); i++)
        {
            SpawnCollectable();
        }
        Destroy(gameObject, 3);
    }

    private void CancelAttack()
    {
        _isAttackInProgress = false;
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
        }
    }

    public Collectable timerCollectablePrefab;
    public Collectable coinCollectablePrefab;

    private void SpawnCollectable()
    {
        Collectable prefab = timerCollectablePrefab;
        if (Random.value < .5f)
        {
            prefab = coinCollectablePrefab;
        }
        var newCollectable = Instantiate(prefab);
        newCollectable.transform.position = transform.position + Vector3.up * 2f 
            + Vector3.right * Random.Range(-.5f, .5f) + Vector3.forward * Random.Range(-.5f, .5f);
        var force = new Vector3(Random.Range(-50f,50f), 200f, Random.Range(-50f, 50f));
        newCollectable.GetComponent<Rigidbody>().AddForce(force);
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

public enum EnemyType
{
    Basic,
    Tough,
    Ranged,
}