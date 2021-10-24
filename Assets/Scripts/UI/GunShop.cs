using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clear.Managers;

namespace Clear.UI
{
    public class GunShop : MonoBehaviour
    {
        [Header("Gun template.")]
        [SerializeField]
        private GunItemObject gunItemTemplate;

        [Header("Itens parent.")]
        [SerializeField]
        private Transform gunItemsParent;

        [Header("PopUps Text")]
        [SerializeField]
        private GameObject notEnoughMoneyObject;
        [SerializeField]
        private GameObject dontHaveStockObject;

        public List<GunItemObject> gunsItensList;

        private void Start()
        {
            gunsItensList = new List<GunItemObject>();
        }

        public void CreateGunItem(int position, GunSO item)
        {
            GunItemObject gun = Instantiate(gunItemTemplate, gunItemsParent);
            gun.gameObject.name = item.gunName + "GunObject";
            gunsItensList.Add(gun);
            gun.Init(position, item);
        }

        public void SetNotEnoughText(Transform rect)
        {
            Transform parent = rect.GetComponent<Transform>();
            GameObject notEnoughObject = Instantiate(notEnoughMoneyObject, parent);
            Destroy(notEnoughObject, 0.5f);
        }

        public void SetDontHaveStock(Transform rect)
        {
            Transform parent = rect.GetComponent<Transform>();
            GameObject dontHaveStock = Instantiate(dontHaveStockObject, parent);
            Destroy(dontHaveStock, 0.8f);
        }
    }
}