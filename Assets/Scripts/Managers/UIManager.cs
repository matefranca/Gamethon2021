using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Clear.Managers
{
    public class UIManager : SingletonInstance<UIManager>
    {
        [Header("Text.")]
        [SerializeField]
        private TMP_Text enemiesLeftText;
        [SerializeField]
        private TMP_Text startGameText;
        [SerializeField]
        private TMP_Text goldAmountText;
        [SerializeField]
        private TMP_Text ammoText;

        [Header("Dialogue Text.")]
        [SerializeField]
        private TMP_Text dialogueText;
        [SerializeField]
        private GameObject spaceBarSkipObject;

        [Header("Shop View.")]
        [SerializeField]
        private GameObject shopView;

        [Header("Dialogue Object.")]
        [SerializeField]
        private Animator dialogueAnimator;

        [Header("Guns Images.")]
        [SerializeField]
        private GameObject[] gunsObjects;

        [Header("Buttons.")]
        [SerializeField]
        private GameObject buttonsParent;

        private string disableStartTextFuncName = "DisableStartText";

        private void Start()
        {
            shopView.SetActive(false);
            DisableStartText();
            DisableButtons();

            PlayerEconomyManager.GetInstance().onGoldCurrencyChanged += UpdateGoldText;
        }

        public void EnableStartText()
        {
            startGameText.gameObject.SetActive(true);
            Invoke(disableStartTextFuncName, 0.5f);
        }

        public void OpenShop() => shopView.SetActive(true);

        public void CloseShop() => shopView.SetActive(false);

        private void UpdateGoldText(int ammount) => goldAmountText.SetText("Gold: " + ammount.ToString());

        public void SetEnemiesLeftText(int amount) => enemiesLeftText.SetText(amount.ToString());

        public void DisableStartText() => startGameText.gameObject.SetActive(false);

        public void OpenDialogue() => dialogueAnimator.SetBool(GameConstants.OPEN_DIALOGUE_BOOL, true);

        public void CloseDialogue() => dialogueAnimator.SetBool(GameConstants.OPEN_DIALOGUE_BOOL, false);

        public void EnableButtons() => buttonsParent.SetActive(true);

        public void DisableButtons() => buttonsParent.SetActive(false);

        public void EnableSpaceBarSkip() => spaceBarSkipObject.SetActive(true);

        public void DisableSpaceBarSkip() => spaceBarSkipObject.SetActive(false);

        public bool IsSpaceBarSkipActive() { return spaceBarSkipObject.activeSelf; }

        public void SetDialogueText(string sentence) => StartCoroutine(TypeText(sentence));

        public void SetAmmoText(int ammo)
        {
            ammoText.SetText(ammo.ToString());
            Color ammoColor = ammo < 5 ? Color.red : Color.yellow;
            ammoText.color = ammoColor;
        }

        private IEnumerator TypeText(string sentence)
        {
            yield return new WaitForSeconds(0.5f);

            dialogueText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }

            EnableSpaceBarSkip();
        }

        public void SetGunObjects(int index, bool active)
        {
            gunsObjects[index].SetActive(active);
        }
    }
}