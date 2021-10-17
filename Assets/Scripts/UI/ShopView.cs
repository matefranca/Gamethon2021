using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Clear.Managers;

namespace Clear.UI
{
    public class ShopView : MonoBehaviour
    {
        [Header("Tabs.")]
        [SerializeField]
        private GameObject gunShopTab;
        [SerializeField]
        private GameObject stockTab;

        [Header("Buttons.")]
        [SerializeField]
        private Button nextLevelButton;



        private void Awake()
        {
            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(StartNextLevel);
        }

        private void OnEnable()
        {
            SwitchTabs(0);
        }

        public void SwitchTabs(int index)
        {
            gunShopTab.SetActive(index == 0);
            stockTab.SetActive(index == 1);
        }

        public void StartNextLevel()
        {
            UIManager.GetInstance().CloseShop();
            GameManager.GetInstance().StartNextLevel();
        }
    }
}