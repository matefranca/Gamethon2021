using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clear.Managers
{
    public class PlayerEconomyManager : SingletonInstance<PlayerEconomyManager>
    {
        public CurrencyData CurrencyData { get; private set; }

        public delegate void OnGoldCurrencyChanged(int ammount);
        public OnGoldCurrencyChanged onGoldCurrencyChanged;

        private void Start()
        {
            CurrencyData = new CurrencyData();
        }

        public void AddGoldCurrency(int ammount)
        {
            CurrencyData.goldAmount += ammount;
            onGoldCurrencyChanged?.Invoke(CurrencyData.goldAmount);
        }

        public void RemoveGoldCurrency(int ammount)
        {
            CurrencyData.goldAmount -= ammount;
            CurrencyData.goldAmount = CurrencyData.goldAmount < 0 ? 0 : CurrencyData.goldAmount;
            onGoldCurrencyChanged?.Invoke(CurrencyData.goldAmount);
        }

        public bool CanAfford(int ammount)
        {
            return ammount <= CurrencyData.goldAmount;
        }
    }

    public class CurrencyData
    {
        public int goldAmount;

        public CurrencyData(int goldAmount)
        {
            this.goldAmount = goldAmount;
        }

        public CurrencyData()
        {
            goldAmount = 0;
        }
    }

}