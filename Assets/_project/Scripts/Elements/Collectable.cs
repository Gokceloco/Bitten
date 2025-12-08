using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float speed;

    public SphereCollider collisionCollider;

    private bool _isMovingTowardsPlayer;
    private Player _player;

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isMovingTowardsPlayer = true;
            _player = other.GetComponent<Player>();
            collisionCollider.enabled = false;
        }
    }

    private void Update()
    {
        if (_isMovingTowardsPlayer)
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
