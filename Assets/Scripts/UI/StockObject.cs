using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Clear.Managers;

namespace Clear.UI
{
    public class StockObject : MonoBehaviour
    {
        [Header("Texts.")]
        [SerializeField]
        private TMP_Text nameText;
        [SerializeField]
        private TMP_Text valueText;
        [SerializeField]
        private TMP_Text quantityText;

        [Header("Input.")]
        [SerializeField]
        private TMP_InputField stockAmount;

        [Header("Buttons.")]
        [SerializeField]
        private Button buyButton;
        [SerializeField]
        private Button sellButton;

        private int stockIndex;
        private int stocksToBuy;

        private void Start()
        {
            stockAmount.onValueChanged.RemoveAllListeners();
            stockAmount.onValueChanged.AddListener(CheckValue);

            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(BuyStock);

            sellButton.onClick.RemoveAllListeners();
            sellButton.onClick.AddListener(SellStock);
        }

        public void Init(int index, StockItemSO item)
        {
            nameText.SetText(item.companyName);
            valueText.SetText(item.initialStockValue.ToString());
            quantityText.SetText("0");
            stockIndex = index;
        }

        public void UpdateValueText(string value)
        {
            valueText.SetText(value);
        }

        public void UpdateQuantityText(string quantity)
        {
            quantityText.SetText(quantity);
        }

        private void CheckValue(string ammount)
        {
            stocksToBuy = int.Parse(ammount);
        }

        private void BuyStock()
        {
            int stocks = stocksToBuy <= 0 ? 1 : stocksToBuy;
            stocksToBuy = 1;
            stockAmount.text = "1";
            if (!StockManager.GetInstance().BuyStock(stockIndex, stocks))
            {
                StockManager.GetInstance().SetNotEnoughText(buyButton.transform);
            }
        }

        private void SellStock()
        {
            int stocks = stocksToBuy <= 0 ? 1 : stocksToBuy;
            stocksToBuy = 1;
            stockAmount.text = "1";
            StockManager.GetInstance().SellStock(stockIndex, stocks);
        }
    }
}