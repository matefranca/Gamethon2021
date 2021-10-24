using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Clear.UI;

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
        private TMP_Text pointsText;
        [SerializeField]
        private TMP_Text lifeText;
        [SerializeField]
        private TMP_Text ammoText;
        [SerializeField]
        private TMP_Text levelText;

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

        [Header("Panels.")]
        [SerializeField]
        private EndPanelView endPanel;
        [SerializeField]
        private GameObject pausePanel;

        [Header("Points Multipliers")]
        [SerializeField]
        private GameObject multiplierObject;
        [SerializeField]
        private TMP_Text multiplierText;

        [Header("Reloading Text.")]
        [SerializeField]
        private GameObject reloadingText;
        [SerializeField]
        private Transform reloadingParent;

        [Header("Pulse Animators.")]
        [SerializeField]
        private Animator goldAnimator;
        [SerializeField]
        private Animator lifeAnimator;

        private string disableStartTextFuncName = "DisableStartText";

        private void Start()
        {
            shopView.SetActive(false);
            DisableStartText();
            DisableButtons();

            PlayerEconomyManager.GetInstance().onGoldCurrencyChanged += UpdateGoldText;
            PlayerManager.GetInstance().onPointsChanged += UpdatePointsText;
        }

        public void EnableStartText()
        {
            startGameText.gameObject.SetActive(true);
            Invoke(disableStartTextFuncName, 0.5f);
        }

        public void OpenShop() => shopView.SetActive(true);

        public void CloseShop() => shopView.SetActive(false);

        private void UpdateGoldText(int ammount) => goldAmountText.SetText("Gold: " + ammount.ToString());

        private void UpdatePointsText(int ammount) => pointsText.SetText("Points: " + ammount.ToString());

        public void SetEnemiesLeftText(int amount) => enemiesLeftText.SetText(amount.ToString());

        public void DisableStartText() => startGameText.gameObject.SetActive(false);

        public void OpenDialogue() => dialogueAnimator.SetBool(GameConstants.OPEN_DIALOGUE_BOOL, true);

        public void CloseDialogue() => dialogueAnimator.SetBool(GameConstants.OPEN_DIALOGUE_BOOL, false);

        public void EnableButtons() => buttonsParent.SetActive(true);

        public void DisableButtons() => buttonsParent.SetActive(false);

        public bool GetButtonsParent() => buttonsParent.activeSelf;

        public void EnableSpaceBarSkip() => spaceBarSkipObject.SetActive(true);

        public void DisableSpaceBarSkip() => spaceBarSkipObject.SetActive(false);

        public bool IsSpaceBarSkipActive() { return spaceBarSkipObject.activeSelf; }

        public void SetDialogueText(string sentence) => StartCoroutine(TypeText(sentence));

        public void SetPausePanel(bool active) => pausePanel.SetActive(active);

        public void SetLevelText(string level) => levelText.SetText(level);

        public void SetLifeText(int life)
        {
            lifeText.SetText(life.ToString());
            Color lifeColor = life <= 1 ? Color.red : Color.yellow;
            lifeText.color = lifeColor;
        }

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

        public void SetEndPanel(bool open, string points, string level)
        {
            endPanel.gameObject.SetActive(open);
            if (open)
            {
                endPanel.Init(points, level);
            }
        }

        public void SetMultipliers(int multiplier)
        {
            multiplierObject.SetActive(multiplier > 1 && multiplier <= 4);

            multiplierText.SetText("x" + multiplier);
        }

        public void CreateReloadingText()
        {
            Destroy(Instantiate(reloadingText, reloadingParent), 0.5f);
        }

        public void SetPulse(PulseObjects pulseObject)
        {
            switch (pulseObject)
            {
                case PulseObjects.life:
                    lifeAnimator.SetTrigger(GameConstants.PULSE_TRIGGER);
                    break;
                case PulseObjects.gold:
                    goldAnimator.SetTrigger(GameConstants.PULSE_TRIGGER);
                    break;
            }
        }
    }

    public enum PulseObjects
    { 
        life,
        gold
    }

}