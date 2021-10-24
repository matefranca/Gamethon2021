using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Clear.Managers
{
    public class MenuManager : MonoBehaviour
    {
        [Header("Texts.")]
        [SerializeField]
        private TMP_Text recordPointsText;
        [SerializeField]
        private TMP_Text recordLevelText;
        [SerializeField]
        private TMP_Text soundText;

        [Header("Buttons.")]
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private Button toggleSoundButton;
        [SerializeField]
        private Button helpButton;
        [SerializeField]
        private Button closeHelpButton;

        [Header("Panels.")]
        [SerializeField]
        private GameObject helpPanel;

        private bool soundActive;

        private void Start()
        {
            if (AudioManager.GetInstance())
            {
                soundActive = AudioManager.GetInstance().Canplay;
                if (!AudioManager.GetInstance().IsPlaying(GameConstants.MENU_SOUND_NAME))
                    AudioManager.GetInstance().Play(GameConstants.MENU_SOUND_NAME);
            }
            else
                soundActive = true;

            SetSound();

            toggleSoundButton.onClick.RemoveAllListeners();
            toggleSoundButton.onClick.AddListener(ToggleSound);

            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(PlayButton);

            helpButton.onClick.RemoveAllListeners();
            helpButton.onClick.AddListener(OpenHelpPanel);

            closeHelpButton.onClick.RemoveAllListeners();
            closeHelpButton.onClick.AddListener(CloseHelpPanel);

            SetRecord();
        }

        private void SetRecord()
        {
            int points = PlayerPrefs.GetInt(GameConstants.POINTS_PLAYER_PREFS, 0);
            int level = PlayerPrefs.GetInt(GameConstants.LEVEL_PLAYER_PREFS, 0);
            recordPointsText.SetText(points.ToString());
            recordLevelText.SetText(level.ToString());
        }    

        
        public void PlayButton()
        {
            Debug.Log("Play");
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            AudioManager.GetInstance().Stop(GameConstants.MENU_SOUND_NAME);
            SceneLoader.GetInstance().LoadScene(GameConstants.GAME_SCENE_NAME);
        }

        public void ToggleSound()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            soundActive = !soundActive;
            SetSound();
        }

        private void SetSound()
        {
            if (soundActive) PlaySound();
            else StopAllSounds();

            string text = soundActive ? GameConstants.SOUND_ON : GameConstants.SOUND_OFF;
            soundText.SetText(text);
        }
        public void StopAllSounds()
        {
            if (!AudioManager.GetInstance()) return;

            AudioManager.GetInstance().Stop(GameConstants.MENU_SOUND_NAME);
            AudioManager.GetInstance().SetCanPlay(false);
        }

        public void PlaySound()
        {
            if (!AudioManager.GetInstance()) return;

            AudioManager.GetInstance().SetCanPlay(true);
            if (!AudioManager.GetInstance().IsPlaying(GameConstants.MENU_SOUND_NAME))
            {
                AudioManager.GetInstance().Play(GameConstants.MENU_SOUND_NAME);
            }
        }

        public void OpenHelpPanel()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            helpPanel.SetActive(true);
        }

        public void CloseHelpPanel()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            helpPanel.SetActive(false);
        }
    }
}