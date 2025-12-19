using UnityEngine;

public class EnemySpell : MonoBehaviour
{
    public float speed;
    public int damage;

    private Player _player;
    private Vector3 _direction;
    public void StartEnemySpell(Player player, Vector3 dir)
    {
        _player = player;
        _direction = dir;
    }

    private void Update()
    {
        transform.position += _direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.gameDirector.fXManager.PlayEnemySpellImpactFX(transform.position);
            other.GetComponent<Player>().GetHit(damage);
            Destroy(gameObject);
        }
        if (other.CompareTag("Wall"))
        {
            _player.gameDirector.fXManager.PlayEnemySpellImpactFX(transform.position);
            Destroy(gameObject);
        }
    }
}
