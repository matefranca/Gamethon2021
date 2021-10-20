using System;
using System.Collections;
using System.Collections.Generic;
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

        [Header("Prefabs.")]
        [SerializeField]
        private GameObject bulletObject;

        [Header("Fire Point")]
        [SerializeField]
        private Transform firePoint;

        private PlayerInput playerInput;

        private GunManager gunManager;
        private UIManager uiManager;

        public bool canShoot = true;

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
            activeGunIndex = 1;
            activeGunSO = activeGunSO = gunManager.GetGunSO(0);
            UpdateGunStatus();
        }

        

        private void ChangeToWeapon(int index)
        {
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

            if (ammo <= 0)
            {
                canShoot = false;
                Invoke(reloadFuncName, reloadTime);
                return;
            }

            GameObject shot = Instantiate(bulletObject, firePoint.position, Quaternion.identity);
            shot.transform.rotation = transform.rotation;
            shot.GetComponent<Rigidbody>().AddForce(transform.forward * fireForce);
            shot.GetComponent<Bullet>().Init(gunDamage);
            Destroy(shot, deathTimer);

            ammo--;
            uiManager.SetAmmoText(ammo);

            canShoot = false;
            Invoke(enableShootFuncName, 1 / fireRate);

            if (AudioManager.GetInstance()) AudioManager.GetInstance().Play(GameConstants.SHOOT_SOUND_NAME);
        }

        private void UpdateGunStatus()
        {
            fireRate = activeGunSO.fireRate;
            gunDamage = activeGunSO.gunDamage;
            reloadTime = activeGunSO.reloadTime;

            ammo = gunManager.GetCurrentGunAmmo(activeGunIndex);
            uiManager.SetAmmoText(ammo);
        }

        private void Reload()
        {
            canShoot = true;
            ammo = activeGunSO.maxAmmo;
            uiManager.SetAmmoText(ammo);
        }

        private void EnableShoot() => canShoot = true;
    }
}