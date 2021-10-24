using System.Collections.Generic;
using UnityEngine;
using Clear.UI;

namespace Clear.Managers
{
    public class GunManager : SingletonInstance<GunManager>
    {
        [Header("Guns SO.")]
        [SerializeField]
        private GunSO[] gunsSO;

        [Header("Gun Shop.")]
        [SerializeField]
        private GunShop gunShop;

        [Header("Projectiles.")]
        [SerializeField]
        private GameObject[] projectiles;
        [SerializeField]
        private GameObject explosion;
        

        private Dictionary<int, bool> gunsOwned;
        private Dictionary<int, int> ammoDictionary;

        protected override void OnInitialize()
        {
            InitializeGunsDictionary();
            InitializeAmmoDictionary();

        }

        private void Start()
        {
            CreateGunItemsObjects();
        }

        private void InitializeGunsDictionary()
        {
            gunsOwned = new Dictionary<int, bool>();
            for (int i = 0; i < gunsSO.Length; i++)
            {
                gunsOwned[i] = false;
            }
            gunsOwned[0] = true;
        }

        private void InitializeAmmoDictionary()
        {
            ammoDictionary = new Dictionary<int, int>();
            for (int i = 0; i < gunsSO.Length; i++)
            {
                ammoDictionary[i] = gunsSO[i].maxAmmo;
            }
        }

        private void CreateGunItemsObjects()
        {
            for (int i = 1; i < gunsSO.Length; i++)
            {
                gunShop.CreateGunItem(i - 1, gunsSO[i]);
            }
        }

        public bool HasGunEnabled(int index)
        {
            return gunsOwned[index];
        }

        public GunSO GetGunSO(int index)
        {
            return gunsSO[index];
        }

        public void UpdateGunsImages()
        {
            foreach (int gun in gunsOwned.Keys)
            {
                UIManager.GetInstance().SetGunObjects(gun, gunsOwned[gun]);
            }
        }

        public bool BuyGun(int index, out bool hasStock)
        {
            if (!StockManager.GetInstance().HasCompanyStock(index - 1))
            {
                hasStock = false;
                return false;
            }

            hasStock = true;

            if (index > 0 && index < gunsOwned.Values.Count)
            {
                int price = gunsSO[index].price;
                if (PlayerEconomyManager.GetInstance().CurrencyData.goldAmount >= price)
                {
                    gunsOwned[index] = true;
                    UpdateGunsImages();
                    Debug.Log("Owned Guns " + index + " = " + gunsOwned[index]);
                    PlayerEconomyManager.GetInstance().RemoveGoldCurrency(price);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void SetNotEnoughText(Transform rect)
        {
            gunShop.SetNotEnoughText(rect);
        }

        public void SetDontHaveStock(Transform rect)
        {
            gunShop.SetDontHaveStock(rect);
        }
        

        public int GetCurrentGunAmmo(int index)
        {
            return ammoDictionary[index];
        }

        public void SetGunAmmo(int index, int ammount)
        {
            ammoDictionary[index] = ammount;
        }

        public GameObject GetBulletPrefab(GunSO gunSO)
        {
            switch (gunSO.projectile)
            {
                case Projectile.bullet:
                    return projectiles[0];

                case Projectile.missile:
                    return projectiles[1];

                default:
                    return null;
            }
        }

        public GameObject GetExplosionObject()
        {
            return explosion;
        }
    }
}