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
        var spread = 0f;
        var attackRateNurf = 0f;
        if (gameDirector.player.GetCurrentDirection().magnitude > 0)
        {
            spread = .15f;
            attackRateNurf = .1f;
        }
        _timeSinceLastShoot += Time.deltaTime;
        if (gameDirector.gameState == GameState.GamePlay && Input.GetMouseButton(0) 
            && _timeSinceLastShoot > attackRate + attackRateNurf)
        {
            Shoot(spread);
        }
    }

    public void Shoot(float spread)
    {
        var newBullet = Instantiate(bulletPrefab);
        newBullet.transform.position = shootPosition.position;
        newBullet.transform.LookAt(shootPosition.position + shootPosition.forward 
            + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0));
        newBullet.StartBullet(this);
        _timeSinceLastShoot = 0;
        muzzlePS.Play();
        muzzleLight.DOIntensity(50, .05f).SetLoops(2, LoopType.Yoyo);
        gameDirector.audioManager.PlayShootAS();
    }
}


public enum WeaponType
{
    MachineGun,
    Shotgun,
}