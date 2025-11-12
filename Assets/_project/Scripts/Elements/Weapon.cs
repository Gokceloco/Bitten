using DG.Tweening;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameDirector gameDirector;
    public WeaponType weaponType;

    public Bullet bulletPrefab;
    public Transform shootPosition;

    public float attackRate;
    private float _timeSinceLastShoot;

    public ParticleSystem muzzlePS;
    public Light muzzleLight;


    private void Update()
    {
        _timeSinceLastShoot += Time.deltaTime;
        if (Input.GetMouseButton(0) && _timeSinceLastShoot > attackRate)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        var newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = shootPosition.position;
        newBullet.transform.LookAt(shootPosition.position + shootPosition.forward);
        newBullet.StartBullet(this);
        _timeSinceLastShoot = 0;
        muzzlePS.Play();
        muzzleLight.DOIntensity(50, .05f).SetLoops(2, LoopType.Yoyo);
    }
}


public enum WeaponType
{
    MachineGun,
    Shotgun,
}