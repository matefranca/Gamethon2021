using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clear.Managers;
using TMPro;

namespace Clear.UI
{
    public class ShopView : MonoBehaviour
    {
        [Header("Tabs.")]
        [SerializeField]
        private GameObject gunShopTab;
        [SerializeField]
        private GameObject stockTab;

        [Header("Gold Ammount Text.")]
        [SerializeField]
        private TMP_Text goldAmountText;

        private void OnEnable()
        {
            SwitchTabs(0);
            UpdateGoldAmountText(PlayerEconomyManager.GetInstance().CurrencyData.goldAmount);
        }

        private void Start()
        {
            PlayerEconomyManager.GetInstance().onGoldCurrencyChanged += UpdateGoldAmountText;
        }

        public void SwitchTabs(int index)
        {
            gunShopTab.SetActive(index == 0);
            stockTab.SetActive(index == 1);
        }

        private void UpdateGoldAmountText(int ammount)
        {
            goldAmountText.SetText("Gold: " + ammount.ToString());
        }
    }
}