using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [Header("Not Enough Money Text.")]
        [SerializeField]
        private GameObject notEnoughMoneyObject;

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
    }
}