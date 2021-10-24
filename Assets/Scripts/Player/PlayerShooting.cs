using UnityEngine;
using Clear.Managers;

namespace Clear
{
    public class PlayerShooting : MonoBehaviour
    {
        [Header("Shooting Attributes.")]
        [SerializeField]
        private float fireForce;
        [SerializeField]
        private float deathTimer;

        [Header("Guns.")]
        [SerializeField]
        private GameObject[] gunsObjects;

        [Header("Fire Point")]
        [SerializeField]
        private Transform firePoint;
        [SerializeField]
        private Transform gunsParent;
        [SerializeField]
        private ParticleSystem muzzleParticle;
        [SerializeField]
        private GameObject smokeObject;

        public Transform FirePoint { get { return firePoint; } }
        public Transform GunsParent { get { return gunsParent; } }

        private PlayerInput playerInput;

        private GunManager gunManager;
        private UIManager uiManager;

        private bool canShoot = true;
        private bool isReloading;

        private float fireRate;
        private int gunDamage;
        private int ammo;
        private float reloadTime;

        private string enableShootFuncName = "EnableShoot";
        private string reloadFuncName = "Reload";

        private int activeGunIndex;
        private GunSO activeGunSO;

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();

            gunManager = GunManager.GetInstance();
            uiManager = UIManager.GetInstance();

            playerInput.onGunInput += ChangeToWeapon;
            playerInput.onShootInput += Shoot;

            canShoot = true;
            activeGunIndex = 0;
            activeGunSO = activeGunSO = gunManager.GetGunSO(0);
            UpdateGunStatus();
        }

        private void ChangeToWeapon(int index)
        {
            if (isReloading)
            {
                uiManager.CreateReloadingText();
                return;
            }

            if (gunManager.HasGunEnabled(index) && activeGunIndex != index && index >= 0 && index < gunsObjects.Length)
            {
                foreach (GameObject gun in gunsObjects)
                {
                    gun.SetActive(false);
                }

                gunsObjects[index].SetActive(true);

                gunManager.SetGunAmmo(activeGunIndex, ammo);

                activeGunSO = gunManager.GetGunSO(index);
                activeGunIndex = index;
                UpdateGunStatus();
            }
        }

        private void Shoot()
        {
            if (!canShoot) return;

            muzzleParticle.Play();

            if (activeGunSO.projectile == Projectile.laser)
            {
                SpawnLaser();
            }
            else
            {
                GameObject bulletObject = GunManager.GetInstance().GetBulletPrefab(activeGunSO);
                GameObject shot = Instantiate(bulletObject, firePoint.position, Quaternion.identity);
                shot.transform.rotation = transform.rotation;
                shot.GetComponent<Rigidbody>().AddForce(firePoint.forward * fireForce);

                if (activeGunSO.projectile == Projectile.bullet)
                {
                    shot.GetComponent<Bullet>().Init(gunDamage);
                    if (AudioManager.GetInstance()) AudioManager.GetInstance().Play(GameConstants.SHOOT_SOUND_NAME);
                }
                else if (activeGunSO.projectile == Projectile.missile)
                {
                    smokeObject.SetActive(true);
                    shot.GetComponent<Missile>().Init(GunManager.GetInstance().GetExplosionObject());
                    if (AudioManager.GetInstance()) AudioManager.GetInstance().Play(GameConstants.MISILE_SOUND_NAME);
                }
                Destroy(shot, deathTimer);
            }

            ammo--;
            uiManager.SetAmmoText(ammo);

            if (ammo <= 0)
            {
                canShoot = false;
                isReloading = true;
                Invoke(reloadFuncName, reloadTime);
                return;
            }

            canShoot = false;
            Invoke(enableShootFuncName, 1 / fireRate);
        }

        private void UpdateGunStatus()
        {
            fireRate = activeGunSO.fireRate;
            gunDamage = activeGunSO.gunDamage;
            reloadTime = activeGunSO.reloadTime;

            ammo = gunManager.GetCurrentGunAmmo(activeGunIndex);
            uiManager.SetAmmoText(ammo);
        }

        private void SpawnLaser()
        {
            AudioManager.GetInstance().Play(GameConstants.RAYGUN_SOUND_NAME);
        }

        // Called by invoke.
        private void Reload()
        {
            canShoot = true;
            isReloading = false;
            ammo = activeGunSO.maxAmmo;
            uiManager.SetAmmoText(ammo);
            gunManager.SetGunAmmo(activeGunIndex, ammo);
        }

        private void EnableShoot() => canShoot = true;
    }
}