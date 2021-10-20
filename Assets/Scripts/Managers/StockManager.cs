using System.Collections.Generic;
using UnityEngine;
using Clear.UI;

namespace Clear.Managers
{
    public class StockManager : SingletonInstance<StockManager>
    {
        [Header("Stock Shop.")]
        [SerializeField]
        private StockShop stockShop;

        [Header("Stock Items")]
        [SerializeField]
        private StockItemSO[] stockItems;

        public List<int> itemQuantity;
        public List<int> itemPrices;

        private PlayerEconomyManager playerEconomyManager;

        private void Start()
        {
            playerEconomyManager = PlayerEconomyManager.GetInstance();

            InitializeStockShop();
        }

        private void InitializeStockShop()
        {
            itemQuantity.Clear();
            itemPrices.Clear();

            for (int i = 0; i < stockItems.Length; i++)
            {
                stockShop.CreateStockObject(i, stockItems[i]);
                itemQuantity.Add(0);
                itemPrices.Add(stockItems[i].initialStockValue);
            }
        }

        public bool BuyStock(int index, int ammount)
        {
            if (playerEconomyManager.CanAfford(itemPrices[index]))
            {
                itemQuantity[index] += ammount;
                playerEconomyManager.RemoveGoldCurrency(itemPrices[index] * ammount);
                stockShop.UpdateQuantityText(index, itemQuantity[index]);
                Debug.Log("Buy: " + ammount);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SellStock(int index, int ammount)
        {
            if (itemQuantity[index] > 0)
            {
                itemQuantity[index]-= ammount;
                playerEconomyManager.AddGoldCurrency(itemPrices[index] * ammount);
                stockShop.UpdateQuantityText(index, itemQuantity[index]);   
                Debug.Log("Sell: " + ammount);
            }
        }

        public void ChangeStocks()
        {
            // TODO - To be calculated.
            /* 
            for (int i = 0; i < itemPrices.Count; i++)
            {
                int rand = Random.Range(0, 2);
                int multiplier = rand == 0 ? 2 : -2;

                int price = itemPrices[i] * multiplier;

                price = Mathf.Clamp(price, 1, 50);
                itemPrices[i] = price;

                stockShop.UpdateValueText(i, itemPrices[i]);
            }*/            
        }

        public void SetNotEnoughText(Transform rect)
        {
            stockShop.SetNotEnoughText(rect);
        }
    }
}