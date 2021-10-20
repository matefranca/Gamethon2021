using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clear.UI
{
    public class StockShop : MonoBehaviour
    {
        [Header("Stock Objects.")]
        [SerializeField]
        private StockObject stockObjectTemplate;
        [SerializeField]
        private RectTransform stockParent;

        [Header("Padding.")]
        [SerializeField]
        private int padding;

        [Header("Not Enough Money Text.")]
        [SerializeField]
        private GameObject notEnoughMoneyObject;

        public List<StockObject> stockList;

        private void Start()
        {
            stockList = new List<StockObject>();
        }

        public void CreateStockObject(int position, StockItemSO item)
        {
            StockObject stock = Instantiate(stockObjectTemplate, stockParent.transform);
            stock.gameObject.name = item.companyName + "StockObject";
            stockList.Add(stock);
            stock.Init(position, item);
            RectTransform rect = stock.GetComponent<RectTransform>();
            float yPos = -padding * position;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, yPos);

        }

        public void UpdateQuantityText(int index, int ammount)
        {
            if (index < 0 || index >= stockList.Count) return;
            stockList[index].UpdateQuantityText(ammount.ToString());
        }

        public void UpdateValueText(int index, int ammount)
        {
            if (index < 0 || index >= stockList.Count) return;
            stockList[index].UpdateValueText(ammount.ToString());
        }

        public void SetNotEnoughText(Transform rect)
        {
            Transform parent = rect.GetComponent<Transform>();
            GameObject notEnoughObject = Instantiate(notEnoughMoneyObject, parent);
            Destroy(notEnoughObject, 0.5f);
        }
    }
}