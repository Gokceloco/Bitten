using DG.Tweening;
using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float speed;

    public SphereCollider collisionCollider;

    private bool _isMovingTowardsPlayer;
    private Player _player;

    private Rigidbody _rb;

    public float availableTime;
    private bool _isAvailable;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, .5f).SetEase(Ease.OutBack);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isAvailable)
        {
            return;
        }
        if (other.CompareTag("Player") && !_isMovingTowardsPlayer)
        {
            _isMovingTowardsPlayer = true;
            _player = other.GetComponent<Player>();
            collisionCollider.enabled = false;
        }
    }

    private void Update()
    {
        availableTime -= Time.deltaTime;
        if (availableTime <= 0) 
        {
            _isAvailable = true;
        }
        if (_isMovingTowardsPlayer && _isAvailable)
        {
            var directionVector = (_player.transform.position + Vector3.up) - transform.position;
            var direction = directionVector.normalized;
            _rb.linearVelocity = direction * speed;
            if (directionVector.magnitude < 1f)
            {
                Collected();
            }
        }
    }

    private void Collected()
    {
        _player.CollectableCollected();
        Destroy(gameObject);
    }
}
