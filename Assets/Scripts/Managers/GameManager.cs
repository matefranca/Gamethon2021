using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Clear.Managers
{
    public class GameManager : SingletonInstance<GameManager>
    {
        [Header("Levels ")]
        [SerializeField]
        private LevelSO[] levels;

        public Camera CurrentCamera { get; private set; }

        public Transform PlayerTransform { get; private set; }

        public bool PlayEnabled { get; private set; }
        public bool InputEnabled { get; private set; }

        private LevelSO currentLevel;

        private UIManager uiManager;
        private PlayerManager playerManager;
        private SpawnerManager spawnerManager;
        private DialogueManager dialogueManager;

        protected override void OnInitialize()
        {
            CurrentCamera = Camera.main;

            PlayerTransform = GameObject.FindGameObjectWithTag(GameConstants.PLAYER_TAG).transform;
        }

        private void Start()
        {
            uiManager = UIManager.GetInstance();
            playerManager = PlayerManager.GetInstance();
            spawnerManager = SpawnerManager.GetInstance();
            dialogueManager = DialogueManager.GetInstance();

            if (AudioManager.GetInstance())
            {
                if (!AudioManager.GetInstance().IsPlaying(GameConstants.GAME_SOUND_NAME))
                {
                    AudioManager.GetInstance().Play(GameConstants.GAME_SOUND_NAME);
                }
            }

            if (levels.Length == 0) return;

            StartNextLevel();
        }


        public void StartNextLevel()
        {
            if (playerManager.GameData.level >= levels.Length)
            {
                LevelSO levelSO = new LevelSO(false, null, currentLevel.numberOfEnemies + 2, currentLevel.timeBtwnSpawns, EnemyType.Slow);
                currentLevel = levelSO;
            }
            else
            {
                currentLevel = levels[playerManager.GameData.level];
            }

            spawnerManager.SetLevel(currentLevel);
            uiManager.DisableButtons();

            if (currentLevel.hasDialogue)
            {
                StartLevelWithDialogue();
            }

            else
            {
                StartLevel();
            }
        }

        public void StartLevel()
        {
            PlayEnabled = true;
            InputEnabled = true;
            spawnerManager.StartLevel();
        }

        private void StartLevelWithDialogue()
        {
            dialogueManager.SetDialogueSO(currentLevel.dialogueSO);
            dialogueManager.OpenDialogue();
        }

        public void EndWave()
        {
            PlayEnabled = false;
        }

        public void WaveCleared()
        {
            InputEnabled = false;
            playerManager.LevelUp();
            uiManager.EnableButtons();
            ShopManager.GetInstance().ChangeStocks();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}