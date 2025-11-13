using UnityEngine;

public class FXManager : MonoBehaviour
{
    public ParticleSystem impactPS;
    public ParticleSystem zombieImpactPS;

    public FloatingText floatingTextPrefab;

    public void PlayImpactPS(Vector3 pos, Vector3 direction)
    {
        var newPS = Instantiate(impactPS);
        newPS.transform.position = pos - direction * .5f;
        newPS.Play();
    }
    public void PlayZombieImpactPS(Vector3 pos, Vector3 direction)
    {
        var newPS = Instantiate(zombieImpactPS);
        newPS.transform.position = pos;
        newPS.transform.LookAt(pos + direction);
        newPS.Play();
    }

    public void SpawnFloatingText(int damage, Vector3 pos)
    {
        var newText = Instantiate(floatingTextPrefab);
        newText.transform.position = pos + Vector3.up * 2.2f;
        newText.StartFloatingText(damage);
    }
}
