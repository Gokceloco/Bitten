using UnityEngine;

public class Saw : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().GetHit(damage);
        }
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().GetHit(damage);
        }
    }
}
