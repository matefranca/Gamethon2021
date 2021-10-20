using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Clear.Managers;

namespace Clear.UI
{
    public class GunItemObject : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField]
        private TMP_Text gunNameText;
        [SerializeField]
        private TMP_Text fireRateText;
        [SerializeField]
        private TMP_Text damageText;
        [SerializeField]
        private TMP_Text priceText;
        [SerializeField]
        private Image gunImage;

        [Header("Buttons.")]
        [SerializeField]
        private Button buyButton;

        private int gunItemIndex;

        private void Start()
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuyGun);
        }

        public void Init(int index, GunSO gunSO)
        {
            gunItemIndex = index;
            gunNameText.SetText(gunSO.gunName);
            fireRateText.SetText("Fire Rate: " + gunSO.fireRate.ToString() + "/s");
            damageText.SetText("Dano: " + gunSO.gunDamage.ToString());
            priceText.SetText("Preço: R$ " + gunSO.price.ToString());
            gunImage.sprite = gunSO.gunImage;
        }

        private void BuyGun()
        {
            if (GunManager.GetInstance().BuyGun(gunItemIndex))
            {
                gameObject.SetActive(false);
            }
            else
            {
                GunManager.GetInstance().SetNotEnoughText(buyButton.transform);
            }
        }
    }
}