using UnityEngine;
using Clear.Managers;
using UnityEngine.UI;
using TMPro;

namespace Clear.UI
{
    public class PausePanelView : MonoBehaviour
    {
        [Header("Buttons.")]
        [SerializeField]
        private Button menuButton;
        [SerializeField]
        private Button retryButton;
        [SerializeField]
        private Button soundEffectButton;
        [SerializeField]
        private Button musicButton;
        [SerializeField]
        private Button closeButton;

        [Header("Texts.")]
        [SerializeField]
        private TMP_Text soundEffectsText;
        [SerializeField]
        private TMP_Text musicText;

        private bool soundEffectActive;
        private bool musicActive;

        private GameManager gameManager;

        private void Start()
        {
            if (AudioManager.GetInstance())
                soundEffectActive = AudioManager.GetInstance().Canplay;
            else
                soundEffectActive = true;

            musicActive = true;

            SetSoundEffect();
            SetMusic();

            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(RestartGame);

            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(GoToMenu);

            soundEffectButton.onClick.RemoveAllListeners();
            soundEffectButton.onClick.AddListener(ToggleSoundEffects);

            musicButton.onClick.RemoveAllListeners();
            musicButton.onClick.AddListener(ToggleGameMusic);

            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(ClosePauseMenu);

        }

        public void ToggleSoundEffects()
        {
            soundEffectActive = !soundEffectActive;
            SetSoundEffect();
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
        }

        public void ToggleGameMusic()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            musicActive = !musicActive;
            SetMusic();
        }

        private void SetSoundEffect()
        {
            if (soundEffectActive)
            {
                GameManager.GetInstance().PlaySound(musicActive);
                string text2 = musicActive ? GameConstants.SOUND_ON : GameConstants.SOUND_OFF;
                musicText.SetText(text2);
            }
            else
            {
                musicText.SetText(GameConstants.SOUND_OFF);
                GameManager.GetInstance().StopAllSounds();
            }

            string text = soundEffectActive ? GameConstants.SOUND_ON : GameConstants.SOUND_OFF;
            soundEffectsText.SetText(text);
        }

        private void SetMusic()
        {
            if (!soundEffectActive) return;

            if (musicActive) AudioManager.GetInstance().Play(GameConstants.GAME_SOUND_NAME);
            else AudioManager.GetInstance().Stop(GameConstants.GAME_SOUND_NAME);

            string text = musicActive ? GameConstants.SOUND_ON : GameConstants.SOUND_OFF;
            musicText.SetText(text);
        }

        private void GoToMenu()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            GameManager.GetInstance().GoToMenu();
        }

        private void RestartGame()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            GameManager.GetInstance().RestartGame();
        }

        private void ClosePauseMenu()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            GameManager.GetInstance().UnpauseGame();
        }
    }
}