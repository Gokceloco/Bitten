using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public List<Enemy> enemies;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
        foreach (Enemy enemy in enemies) 
        {
            enemy.GetHit(100);
        }
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

    private void OnTriggerStay(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}
