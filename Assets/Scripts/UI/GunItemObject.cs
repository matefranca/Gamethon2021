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
        [SerializeField]
        private TMP_Text companyNameText;

        [Header("Buttons.")]
        [SerializeField]
        private Button buyButton;

        private int gunItemIndex; // 1 less than in the gun manager.
        private int price;

        private StockManager stockManager;
        private PlayerEconomyManager playerEconomyManager;

        private void Start()
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuyGun);

            stockManager = StockManager.GetInstance();
        }

        public void Init(int index, GunSO gunSO)
        {
            price = gunSO.price;
            gunItemIndex = index;
            gunImage.sprite = gunSO.gunImage;
            gunNameText.SetText(gunSO.gunName);
            priceText.SetText("Preço: R$ " + price.ToString());
            companyNameText.SetText(gunSO.stockItemSO.companyName);
            damageText.SetText("Dano: " + gunSO.gunDamage.ToString());
            fireRateText.SetText("Fire Rate: " + gunSO.fireRate.ToString() + "/s");
        }

        private void BuyGun()
        {
            bool hasStock;
            if (GunManager.GetInstance().BuyGun(gunItemIndex + 1, out hasStock))
            {
                AudioManager.GetInstance().Play(GameConstants.BUY_CLICK_SOUND_NAME);
                gameObject.SetActive(false);
            }
            else
            {
                AudioManager.GetInstance().Play(GameConstants.ACESS_DENIED_SOUND_NAME);

                if (!hasStock)
                {
                    GunManager.GetInstance().SetDontHaveStock(buyButton.transform);
                }
                else
                {
                    GunManager.GetInstance().SetNotEnoughText(buyButton.transform);
                }
            }
        }
    }
}