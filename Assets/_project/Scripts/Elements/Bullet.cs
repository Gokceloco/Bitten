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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            _weapon.gameDirector.fXManager.PlayImpactPS(transform.position, transform.forward);
            Destroy(gameObject);
        }
        if (other.CompareTag("Enemy"))
        {
            _weapon.gameDirector.fXManager.PlayZombieImpactPS(transform.position, transform.forward);
            other.GetComponent<Enemy>().GetHit(1);
            Destroy(gameObject);
        }
    }
}
