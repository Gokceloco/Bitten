using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public float shotgunSpread;

    public List<Bullet> bullets;

    private void Awake()
    {
        CreateBullets();
    }

    private void CreateBullets()
    {
        for (int i = 0; i < 50; i++)
        {
            var newBullet = Instantiate(bulletPrefab, transform);
            newBullet.transform.position = shootPosition.position;
            newBullet.gameObject.SetActive(false);
            newBullet.transform.position = shootPosition.position;
            bullets.Add(newBullet);
        }        
    }

    private void Update()
    {
        _timeSinceLastShoot += Time.deltaTime;

        if (weaponType == WeaponType.MachineGun)
        {
            var spread = 0f;
            var attackRateNurf = 0f;
            if (gameDirector.player.GetCurrentDirection().magnitude > 0)
            {
                spread = .15f;
                attackRateNurf = .1f;
            }
            if (gameDirector.gameState == GameState.GamePlay && Input.GetMouseButton(0)
                && _timeSinceLastShoot > attackRate + attackRateNurf
                && !EventSystem.current.IsPointerOverGameObject())
            {
                Shoot(spread);
                muzzlePS.Play();
                muzzleLight.DOIntensity(50, .05f).SetLoops(2, LoopType.Yoyo);
                gameDirector.audioManager.PlayShootAS();
            }
        }
        else if (weaponType == WeaponType.Shotgun)
        {
            if (gameDirector.gameState == GameState.GamePlay && Input.GetMouseButtonUp(0)
                && _timeSinceLastShoot > attackRate
                && !EventSystem.current.IsPointerOverGameObject())
            {
                for (global::System.Int32 i = 0; i < 20; i++)
                {
                    Shoot(shotgunSpread);
                }
                muzzlePS.Play();
                muzzleLight.DOIntensity(50, .05f).SetLoops(2, LoopType.Yoyo);
                gameDirector.audioManager.PlayShotgunShootAS();
            }
        }
    }

    public void Shoot(float spread)
    {
        var newBullet = bullets[0];
        newBullet.gameObject.SetActive(true);
        newBullet.transform.SetParent(null);
        newBullet.transform.position = shootPosition.position;
        newBullet.transform.LookAt(shootPosition.position + shootPosition.forward 
            + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0));
        newBullet.StartBullet(this);        
        bullets.Remove(newBullet);
        _timeSinceLastShoot = 0;        
    }
}


public enum WeaponType
{
    MachineGun,
    Shotgun,
}