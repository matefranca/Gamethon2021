using UnityEngine;
using Clear.Data;
using Clear.Utils;

namespace Clear.Managers
{
    public class PlayerManager : SingletonInstance<PlayerManager>
    {
        [Header("Health")]
        [SerializeField]
        private int initialHealth;

        [Header("Points Thresholds.")]
        [SerializeField]
        private int x2 = 5;
        [SerializeField]
        private int x3 = 10;
        [SerializeField]
        private int x4 = 15;

        public GameData GameData { get; private set; }

        private UIManager uiManager;

        public delegate void OnPointsChanged(int points);
        public OnPointsChanged onPointsChanged;

        private int enemiesKilled;
        private int multiplier;

        protected override void OnInitialize()
        {
            GameData = new GameData(initialHealth);
        }

        private void Start()
        {
            multiplier = 1;

            uiManager = UIManager.GetInstance();
            uiManager.SetLifeText(GameData.health);
            uiManager.SetMultipliers(multiplier);
        }

        public void LevelUp()
        {
            GameData.level++;
        }

        public void TakeDamage(int ammount)
        {
            if (!GameManager.GetInstance().InputEnabled) return;

            CameraController.GetInstance().Shake();
            AudioManager.GetInstance().Play(GameConstants.PLAYER_HIT_SOUND_NAME);
            uiManager.SetPulse(PulseObjects.life);

            GameData.health -= ammount;
            GameData.health = GameData.health <= 0 ? 0 : GameData.health;
            uiManager.SetLifeText(GameData.health);
            ResetEnemiesKilled();

            if (GameData.health <= 0)
            {
                GameManager.GetInstance().EndGame(GameData.points.ToString(), GameData.level.ToString());
            }

        }

        public void AddPoints(int points)
        {
            GameData.points += points;
            onPointsChanged?.Invoke(GameData.points);
        }

        public void AddEnemiesKilled()
        {
            enemiesKilled++;

            if (enemiesKilled >= x2 && enemiesKilled < x3) multiplier = 2;
            else if (enemiesKilled >= x3 && enemiesKilled < x4) multiplier = 3;
            else if (enemiesKilled >= x4) multiplier = 4;
            else multiplier = 1;
            uiManager.SetMultipliers(multiplier);
        }

        public void ResetEnemiesKilled()
        {
            enemiesKilled = 0;
            multiplier = 1;
            uiManager.SetMultipliers(multiplier);
        }

        public void CheckForRecord()
        {
            int pointsRecord = PlayerPrefs.GetInt(GameConstants.POINTS_PLAYER_PREFS, 0);
            int levelRecord = PlayerPrefs.GetInt(GameConstants.LEVEL_PLAYER_PREFS, 0);

            pointsRecord = pointsRecord < GameData.points ? GameData.points : pointsRecord;
            levelRecord = levelRecord < GameData.level ? GameData.level : levelRecord;

            PlayerPrefs.SetInt(GameConstants.POINTS_PLAYER_PREFS, pointsRecord);
            PlayerPrefs.SetInt(GameConstants.LEVEL_PLAYER_PREFS, levelRecord);
        }
    }
}