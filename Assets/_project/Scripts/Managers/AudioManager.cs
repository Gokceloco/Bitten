using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource shootAS;
    public AudioSource zombieImpactAS;

    public AudioSource ambient1AS;
    public AudioSource ambient2AS;

    public AudioSource victory1AS;
    public AudioSource victory2AS;

    public AudioSource fail1AS;
    public AudioSource fail2AS;

    public AudioSource timeTickAS;

    public AudioSource zombieScreamAS;

    public void PlayShootAS()
    {
        shootAS.Play();
    }
    public void PlayHitAS()
    {
        zombieImpactAS.Play();
    }

    public void PlayTimeTickAS()
    {
        timeTickAS.Play();
    }

    public void PlayAmbientSound()
    {
        if (Random.value < .5f)
        {
            ambient1AS.Play();
        }
        else
        {
            ambient2AS.Play();
        }
    }

    public void StopAmbientSound()
    {
        ambient1AS.Stop();
        ambient2AS.Stop();
    }

    public void PlayVictoryAS()
    {
        if (Random.value < .5f)
        {
            victory1AS.Play();
        }
        else
        {
            victory2AS.Play();
        }
    }

    public void PlayFailAS()
    {
        if (Random.value < .5f)
        {
            fail1AS.Play();
        }
        else
        {
            fail2AS.Play();
        }
    }

    public void PlayZombieScreamAS()
    {
        zombieScreamAS.Play();
    }
}
