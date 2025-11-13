using System.Collections;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public ParticleSystem impactPS;
    public ParticleSystem zombieImpactPS;
    public ParticleSystem potionCollectPS;
    public ParticleSystem zombieExpirePS;

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

    public void PlayPotionCollectPS(Vector3 pos)
    {
        var newPS = Instantiate(potionCollectPS);
        newPS.transform.position = pos + Vector3.up;
        newPS.Play();
    }

    public void PlayZombieDestroyPSDelayed(float delay, Transform chestBone)
    {
        StartCoroutine(PlayZombieExpirePS(delay, chestBone));
    }

    IEnumerator PlayZombieExpirePS(float delay, Transform chestBone)
    {
        yield return new WaitForSeconds(delay);
        var newPS = Instantiate(zombieExpirePS);
        newPS.transform.position = chestBone.position;
        newPS.Play();
    }
}
