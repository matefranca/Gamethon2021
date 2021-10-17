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
        private GameObject firstGunObject;
        [SerializeField]
        private GameObject secondGunObject;

        [Header("Prefabs.")]
        [SerializeField]
        private GameObject bulletObject;

        [Header("Fire Point")]
        [SerializeField]
        private Transform firePoint;

        [Header("Guns SO.")]
        [SerializeField]
        private GunSO[] gunsSO;

        private PlayerInput playerInput;

        public bool canShoot = true;

        private float fireRate;
        private int gunDamage;

        private string enableShootFuncName = "EnableShoot";

        private int activeWeapon;
        private GunSO activeGunSO;

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            playerInput.onFirstGunInput += ChangeToFirstWeapon;
            playerInput.onSecondGunInput += ChangeToSecondWeapon;

            playerInput.onShootInput += Shoot;
            canShoot = true;
            activeWeapon = 1;
            activeGunSO = gunsSO[0];
            UpdateGunStatus();
        }

        private void ChangeToFirstWeapon()
        {
            if (activeWeapon == 1) return;

            firstGunObject.SetActive(true);
            secondGunObject.SetActive(false);
            activeWeapon = 1;
            activeGunSO = gunsSO[0];
            UpdateGunStatus();
        }

        private void ChangeToSecondWeapon()
        {
            if (activeWeapon == 2) return;

            firstGunObject.SetActive(false);
            secondGunObject.SetActive(true);
            activeWeapon = 2;
            activeGunSO = gunsSO[1];
            UpdateGunStatus();
        }

        private void Shoot()
        {
            if (!canShoot) return;

            GameObject shot = Instantiate(bulletObject, firePoint.position, Quaternion.identity);
            shot.transform.rotation = transform.rotation;
            shot.GetComponent<Rigidbody>().AddForce(transform.forward * fireForce);
            shot.GetComponent<Bullet>().Init(gunDamage);
            Destroy(shot, deathTimer);

            canShoot = false;
            Invoke(enableShootFuncName, 1 / fireRate);

            if (AudioManager.GetInstance()) AudioManager.GetInstance().Play(GameConstants.SHOOT_SOUND_NAME);
        }
        private void UpdateGunStatus()
        {
            fireRate = activeGunSO.fireRate;
            gunDamage = activeGunSO.gunDamage;
        }

        private void EnableShoot() => canShoot = true;
    }
}