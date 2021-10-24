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
        [SerializeField]
        private GameObject skinTab;

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
            stockTab.SetActive(index == 0);
            gunShopTab.SetActive(index == 1);
            skinTab.SetActive(index == 2);
        }

        private void UpdateGoldAmountText(int ammount)
        {
            goldAmountText.SetText("Gold: " + ammount.ToString());
        }

        public void PlayButtonSound()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
        }
    }
}