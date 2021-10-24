using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Clear.Managers
{
    public class GameManager : SingletonInstance<GameManager>
    {
        [Header("Levels ")]
        [SerializeField]
        private LevelSO[] levels;

        [Header("Buttons.")]
        [SerializeField]
        private Button nextLevelButton;
        [SerializeField]
        private Button pauseButton;
        [SerializeField]
        private Button openShopButton;

        [Header("Gold Removed.")]
        [SerializeField]
        private int goldRemovedAmmount = 100;

        [Header("Pause Key")]
        [SerializeField]
        private KeyCode escapeKey = KeyCode.Escape;

        public Camera CurrentCamera { get; private set; }

        public Transform PlayerTransform { get; private set; }

        public bool SpawnEnabled { get; private set; }
        public bool InputEnabled { get; private set; }

        private LevelSO currentLevelSO;
        private int levelSOindex;
        private int nextLevelSO;

        private UIManager uiManager;
        private GunManager gunManager;
        private PlayerManager playerManager;
        private SpawnerManager spawnerManager;
        private DialogueManager dialogueManager;

        private bool canStartNextLevel;

        protected override void OnInitialize()
        {
            CurrentCamera = Camera.main;

            PlayerTransform = GameObject.FindGameObjectWithTag(GameConstants.PLAYER_TAG).transform;

            nextLevelButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.AddListener(StartNextLevelButton);

            pauseButton.onClick.RemoveAllListeners();
            pauseButton.onClick.AddListener(PauseGame);

            openShopButton.onClick.RemoveAllListeners();
            openShopButton.onClick.AddListener(OpenShop);
        }

        private void Start()
        {
            Time.timeScale = 1;

            uiManager = UIManager.GetInstance();
            gunManager = GunManager.GetInstance();
            playerManager = PlayerManager.GetInstance();
            spawnerManager = SpawnerManager.GetInstance();
            dialogueManager = DialogueManager.GetInstance();

            uiManager.SetEndPanel(false, "0", "0");

            if (AudioManager.GetInstance())
            {
                if (!AudioManager.GetInstance().IsPlaying(GameConstants.GAME_SOUND_NAME))
                {
                    AudioManager.GetInstance().Play(GameConstants.GAME_SOUND_NAME);
                }
            }

            if (levels.Length == 0) return;
            currentLevelSO = levels[0];
            nextLevelSO = levels[1].initalLevel;

            gunManager.UpdateGunsImages();
            uiManager.SetLevelText("1");

            canStartNextLevel = true;
            StartNextLevel();
        }

        private void Update()
        {
            if (uiManager.GetButtonsParent())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartNextLevel();
                }
            }

            if (Input.GetKeyDown(escapeKey))
            {
                PauseGame();
            }
        }

        private void StartNextLevelButton()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            StartNextLevel();
        }


        public void StartNextLevel()
        {
            if (!canStartNextLevel) return;

            canStartNextLevel = false;
            Debug.Log("Starting New Level");

            uiManager.CloseShop();
            uiManager.DisableButtons();


            // Check for Level SO.
            int level = playerManager.GameData.level;
            if (level == nextLevelSO)
            {
                levelSOindex++;

                if (levelSOindex == levels.Length - 1)
                {
                    //currentLevelSO = levels[levelSOindex];
                    nextLevelSO = -1;
                }
                else
                {
                    currentLevelSO = levels[levelSOindex];
                    nextLevelSO = levels[levelSOindex + 1].initalLevel;
                }
            }
            else if (level != 1)
            {
                //LevelSO levelSO = new LevelSO(false, null, currentLevelSO.numberOfEnemies + 2, currentLevelSO.timeBtwnSpawns - 0.2f, currentLevelSO.enemyType);
                LevelSO levelSO = new LevelSO(false, null, currentLevelSO.numberOfEnemies, currentLevelSO.timeBtwnSpawns - 0.2f, currentLevelSO.enemyType);
                currentLevelSO = levelSO;
            }

            // Check for Start Level.
            if (level % 5 == 0) // Every 5 level
            {
                StartLevelWithBoss();
            }
            else if (nextLevelSO == -1)
            {
                GenerateRandomLevel(level, currentLevelSO.timeBtwnSpawns);
            }
            else
            {
                spawnerManager.SetLevel(currentLevelSO);
                if (currentLevelSO.hasDialogue)
                {
                    StartLevelWithDialogue();
                }
                else
                {
                    StartLevel();
                }
            }

            uiManager.SetLevelText(level.ToString());
        }

        private void EnableInputs()
        {
            SpawnEnabled = true;
            InputEnabled = true;
        }

        public void StartLevel()
        {
            EnableInputs();
            spawnerManager.StartLevel();
        }

        private void StartLevelWithDialogue()
        {
            dialogueManager.SetDialogueSO(currentLevelSO.dialogueSO);
            dialogueManager.OpenDialogue();
        }

        private void StartLevelWithBoss()
        {
            EnableInputs();
            spawnerManager.StartLevelWithBoss(playerManager.GameData.level / 5);
        }

        public void GenerateRandomLevel(int level, float initialSpawnTime)
        {
            EnableInputs();
            spawnerManager.StartRandomLevel(level, initialSpawnTime);
        }

        public void EndWave()
        {
            SpawnEnabled = false;
        }

        public void WaveCleared()
        {
            Debug.Log("Wave Cleared.");

            InputEnabled = false;
            canStartNextLevel = true;

            playerManager.LevelUp();
            uiManager.EnableButtons();
            StockManager.GetInstance().ChangeStocks();
        }

        public void TakeMoney()
        {
            if (!InputEnabled) return;

            AudioManager.GetInstance().Play(GameConstants.PLAYER_HIT_SOUND_NAME);
            PlayerEconomyManager.GetInstance().RemoveGoldCurrency(goldRemovedAmmount);
            UIManager.GetInstance().SetPulse(PulseObjects.gold);
        }

        public void PauseGame()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            Time.timeScale = 0;
            uiManager.SetPausePanel(true);
        }

        public void UnpauseGame()
        {
            Time.timeScale = 1;
            uiManager.SetPausePanel(false);
        }

        public void RestartGame()
        {
            PlayerManager.GetInstance().CheckForRecord();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void EndGame(string points, string level)
        {
            Time.timeScale = 0;
            uiManager.SetEndPanel(true, points, level);
        }

        public void StopAllSounds()
        {
            if (!AudioManager.GetInstance()) return;

            AudioManager.GetInstance().Stop(GameConstants.GAME_SOUND_NAME);
            AudioManager.GetInstance().SetCanPlay(false);
        }

        public void PlaySound(bool musicActive)
        {
            if (!AudioManager.GetInstance()) return;

            AudioManager.GetInstance().SetCanPlay(true);
            if (!AudioManager.GetInstance().IsPlaying(GameConstants.GAME_SOUND_NAME) && musicActive)
            {
                AudioManager.GetInstance().Play(GameConstants.GAME_SOUND_NAME);
            }
        }

        public void GoToMenu()
        {
            AudioManager.GetInstance().Stop(GameConstants.GAME_SOUND_NAME);
            PlayerManager.GetInstance().CheckForRecord();
            SceneLoader.GetInstance().LoadScene(GameConstants.MENU_SCENE_NAME);
        }

        private void OpenShop()
        {
            AudioManager.GetInstance().Play(GameConstants.BUTTON_SELECT_SOUND_NAME);
            uiManager.OpenShop();
        }
    }
}