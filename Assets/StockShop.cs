using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Clear.Managers;
using System;

namespace Clear.UI
{
    public class StockShop : MonoBehaviour
    {
        [Header("Text.")]
        [SerializeField]
        private TMP_Text[] pricesTexts;
        [SerializeField]
        private TMP_Text[] quantityTexts;
        [SerializeField]
        private TMP_Text goldAmountText;

        void Start()
        {
            PlayerEconomyManager.GetInstance().onGoldCurrencyChanged += UpdateGoldAmountText;
        }

        private void OnEnable()
        {
            UpdateGoldAmountText(PlayerEconomyManager.GetInstance().CurrencyData.goldAmount);
        }

        private void UpdateGoldAmountText(int ammount)
        {
            goldAmountText.SetText("Gold: " + ammount.ToString());
        }

        public void UpdateQuantityText(int index, int ammount)
        {
            quantityTexts[index].SetText(ammount.ToString());
        }

        public void UpdatePriceText(int index, int ammount)
        {
            pricesTexts[index].SetText("R$ " + ammount.ToString());
        }
    }
}