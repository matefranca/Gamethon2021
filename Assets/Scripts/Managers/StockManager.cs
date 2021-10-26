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

        private List<int> itemQuantity;
        private List<int> itemPrices;

        private PlayerEconomyManager playerEconomyManager;

        private void Start()
        {
            playerEconomyManager = PlayerEconomyManager.GetInstance();

            InitializeStockShop();
        }

        private void InitializeStockShop()
        {
            itemQuantity = new List<int>();
            itemPrices = new List<int>();

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
                AudioManager.GetInstance().Play(GameConstants.BUY_CLICK_SOUND_NAME);

                itemQuantity[index] += ammount;
                playerEconomyManager.RemoveGoldCurrency(itemPrices[index] * ammount);
                stockShop.UpdateQuantityText(index, itemQuantity[index]);
                Debug.Log("Buy: " + ammount);
                return true;
            }
            else
            {
                AudioManager.GetInstance().Play(GameConstants.ACESS_DENIED_SOUND_NAME);

                return false;
            }
        }

        public void SellStock(int index, int ammount)
        {
            if (itemQuantity[index] >= ammount)
            {
                AudioManager.GetInstance().Play(GameConstants.BUY_CLICK_SOUND_NAME);

                itemQuantity[index] -= ammount;
                playerEconomyManager.AddGoldCurrency(itemPrices[index] * ammount);
                stockShop.UpdateQuantityText(index, itemQuantity[index]);
                Debug.Log("Sell: " + ammount);
            }
            {
                if (itemQuantity[index] > 0)
                {
                    AudioManager.GetInstance().Play(GameConstants.BUY_CLICK_SOUND_NAME);

                    playerEconomyManager.AddGoldCurrency(itemPrices[index] * itemQuantity[index]);
                    stockShop.UpdateQuantityText(index, itemQuantity[index]);
                    Debug.Log("Sell: " + itemQuantity[index]);
                    itemQuantity[index] = 0;
                }
                else
                {
                    AudioManager.GetInstance().Play(GameConstants.ACESS_DENIED_SOUND_NAME);
                }
            }
        }

        public int GetStockValue(int index)
        {
            return itemPrices[index];
        }

        public void SetStockValue(int index, int value)
        {
            itemPrices[index] = value;
        }

        public void ChangeStocks()
        {
            stockShop.ChangeStocks();           
        }

        public void SetNotEnoughText(Transform rect)
        {
            stockShop.SetNotEnoughText(rect);
        }

        public bool HasCompanyStock(int index)
        {
            return itemQuantity[index] > 0;
        }
    }
}