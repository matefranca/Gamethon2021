using System.Collections.Generic;
using UnityEngine;
using Clear.UI;

namespace Clear.Managers
{
    public class ShopManager : SingletonInstance<ShopManager>
    {
        [Header("Stock Shop.")]
        [SerializeField]
        private StockShop stockShop;

        public List<int> itemQuantity;
        public List<int> itemPrices;

        private PlayerEconomyManager playerEconomyManager;

        private void Start()
        {
            playerEconomyManager = PlayerEconomyManager.GetInstance();

            InitializeShop();
        }

        private void InitializeShop()
        {
            for (int i = 0; i < itemQuantity.Count; i++)
            {
                stockShop.UpdatePriceText(i, itemPrices[i]);
                stockShop.UpdateQuantityText(i, itemQuantity[i]);
            }
        }

        public void BuyStock(int index)
        {
            if (playerEconomyManager.CanAfford(itemPrices[index]))
            {
                itemQuantity[index]++;
                playerEconomyManager.RemoveGoldCurrency(itemPrices[index]);
                stockShop.UpdateQuantityText(index, itemQuantity[index]);
            }
        }

        public void SellStock(int index)
        {
            if (itemQuantity[index] > 0)
            {
                itemQuantity[index]--;
                playerEconomyManager.AddGoldCurrency(itemPrices[index]);
                stockShop.UpdateQuantityText(index, itemQuantity[index]);
            }
        }

        public void ChangeStocks()
        {
            // TODO - To be calculated.
             
            for (int i = 0; i < itemPrices.Count; i++)
            {
                int rand = Random.Range(0, 2);
                int multiplier = rand == 0 ? 2 : -2;

                int price = itemPrices[i] * multiplier;

                price = Mathf.Clamp(price, 1, 50);
                itemPrices[i] = price;

                stockShop.UpdatePriceText(i, itemPrices[i]);
            }

            
        }
    }
}