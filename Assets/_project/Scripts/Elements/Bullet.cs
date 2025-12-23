using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Weapon _weapon;
    public float speed;
    public float destroyDistance;

    public void StartBullet(Weapon w)
    {
        _weapon = w;
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        var distance = (transform.position - _weapon.transform.position).magnitude;
        if (distance > destroyDistance)
        {
            AddBackToPool();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            _weapon.gameDirector.fXManager.PlayImpactPS(transform.position, transform.forward);
            AddBackToPool();
        }
        if (other.CompareTag("Enemy"))
        {
            var bonusDamage = _weapon.gameDirector.upgradeManager.attackUpgradeCount;
            var damage = 1 + bonusDamage;
            _weapon.gameDirector.fXManager.PlayZombieImpactPS(transform.position, transform.forward);
            var angle = Vector3.Angle(transform.forward, other.transform.forward);
            if (angle < 90)
            {
                _weapon.gameDirector.audioManager.PlayPositiveAS();
                damage *= 2;
            }
            other.GetComponent<Enemy>().GetHit(damage);
            AddBackToPool();
        }
    }

    private void AddBackToPool()
    {
        _weapon.bullets.Add(this);
        gameObject.SetActive(false);
        transform.position = _weapon.shootPosition.position;
        transform.SetParent(_weapon.transform);
    }
}
