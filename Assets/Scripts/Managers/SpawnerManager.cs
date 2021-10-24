using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clear.Managers
{
    public enum EnemyType
    {
        Fast,
        Slow,
        Money
    }

    public class SpawnerManager : SingletonInstance<SpawnerManager>
    {
        [Header("Spawn Points.")]
        [SerializeField]
        private Transform spawnPointsParent;
        [SerializeField]
        private Transform bossSpawnPoint;

        [Header("Boss Prefab.")]
        [SerializeField]
        private GameObject bossPrefab;

        [Header("Types of enemys.")]
        [SerializeField]
        private EnemySO[] enemiesSO;

        [Header("Spawn Portal.")]
        [SerializeField]
        private GameObject portalSpawner;

        private List<Transform> spawnPointsList;

        private Dictionary<EnemyType, EnemySO> enemiesDictionary;
        private Dictionary<EnemyType, GameObject> enemiesGameObjectsDictionary;

        private int numberOfEnemies;
        private float timeBtwSpawn;
        private EnemyType currentType;
        private bool canSpawn;
        private bool levelStarted;

        private bool isBossLevel;
        private int bossLifeMultiplier;

        private bool isRandomSpawn;
        private int enemies1;
        private int enemies2;
        private int enemies3;

        private List<GameObject> enemiesList;
        private int enemiesSpawned;
        private int enemiesKilled;

        private UIManager uiManager;
        private GameManager gameManager;
        private PlayerManager playerManager;
        private PlayerEconomyManager playerEconomyManager;

        private string enableSpawnFuncName = "EnableSpawn";

        private void Start()
        {
            FillSpawnPoints();
            FillEnemiesDictionary();

            uiManager = UIManager.GetInstance();
            gameManager = GameManager.GetInstance();
            playerManager = PlayerManager.GetInstance();
            playerEconomyManager = PlayerEconomyManager.GetInstance();

            enemiesList = new List<GameObject>();
        }

        private void Update()
        {
            Spawn();
            CheckForEndLevel();
        }

        private void FillSpawnPoints()
        {
            spawnPointsList = new List<Transform>();

            foreach (Transform item in spawnPointsParent)
            {
                if (item == spawnPointsParent) continue;

                spawnPointsList.Add(item);
            }
        }

        private void FillEnemiesDictionary()
        {
            enemiesDictionary = new Dictionary<EnemyType, EnemySO>();
            enemiesGameObjectsDictionary = new Dictionary<EnemyType, GameObject>();

            foreach (EnemySO so in enemiesSO)
            {
                enemiesDictionary[so.enemyType] = so;
                enemiesGameObjectsDictionary[so.enemyType] = so.enemyPrefab;
            }
        }

        public void StartLevel()
        {
            isBossLevel = false;
            isRandomSpawn = false;

            StartCoroutine(StartLevelSequence());
        }

        public void StartLevelWithBoss(int multiplier)
        {
            multiplier = Mathf.Max(multiplier, 1);

            numberOfEnemies = 1;
            enemiesSpawned = 0;
            enemiesKilled = 0;

            isBossLevel = true;
            isRandomSpawn = false;

            bossLifeMultiplier = multiplier;

            StartCoroutine(StartLevelSequence());
        }

        public void StartRandomLevel(int level, float initialSpawnTime)
        {
            numberOfEnemies = Mathf.Min(level, 50);
            timeBtwSpawn = initialSpawnTime - (level / 100);
            timeBtwSpawn = Mathf.Clamp(timeBtwSpawn, 0.5f, 10);

            enemies1 = Random.Range(0, numberOfEnemies);
            enemies2 = Random.Range(0, numberOfEnemies - enemies1);
            enemies3 = numberOfEnemies - enemies1 - enemies2;

            enemiesSpawned = 0;
            enemiesKilled = 0;

            isRandomSpawn = true;
            isBossLevel = false;

            StartCoroutine(StartLevelSequence());
        }

        private IEnumerator StartLevelSequence()
        {
            uiManager.EnableStartText();
            uiManager.SetEnemiesLeftText(numberOfEnemies);

            yield return new WaitForSeconds(1f);

            canSpawn = true;
            levelStarted = true;
        }

        public void SetLevel(LevelSO levelSO)
        {
            numberOfEnemies = levelSO.numberOfEnemies;
            timeBtwSpawn = levelSO.timeBtwnSpawns;
            currentType = levelSO.enemyType;
            enemiesSpawned = 0;
            enemiesKilled = 0;
        }

        private void Spawn()
        {
            if (gameManager.SpawnEnabled && canSpawn)
            {
                canSpawn = false;

                if (isBossLevel)
                {
                    SpawnBoss();
                    gameManager.EndWave();
                }
                else
                {
                    enemiesSpawned++;

                    if (isRandomSpawn)
                    {
                        SpawnEnemiesRandomly();
                    }
                    else
                    {
                        SpawnEnemy(currentType);
                    }

                    Invoke(enableSpawnFuncName, timeBtwSpawn);

                    if (enemiesSpawned == numberOfEnemies)
                        gameManager.EndWave();
                }
            }
        }

        private void SpawnEnemy(EnemyType type)
        {
            int rand = Random.Range(0, spawnPointsList.Count);

            while (Vector3.Distance(spawnPointsList[rand].position, GameManager.GetInstance().PlayerTransform.position) < 6 )
            {
                rand = Random.Range(0, spawnPointsList.Count);
            }

            EnemySO enemySO = enemiesDictionary[type];

            GameObject portalGO = Instantiate(portalSpawner, spawnPointsList[rand].position, Quaternion.identity);
            PortalSpawner portalSpawnerScript = portalGO.GetComponent<PortalSpawner>();

            portalSpawnerScript.Init(enemiesGameObjectsDictionary[type], enemySO); 
        }

        private void SpawnBoss()
        {
            GameObject bossGO = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            enemiesList.Add(bossGO);
            Boss boss = bossGO.GetComponent<Boss>();
            boss.Init(bossLifeMultiplier, GameManager.GetInstance().PlayerTransform);
        }

        private void SpawnEnemiesRandomly()
        {
            bool hasEnemiesToSpawn = true;
            int random;
            EnemyType type;

            do
            {
                random = Random.Range(0, enemiesSO.Length);
                if (random == 0 && enemies1 > 0)
                {
                    enemies1--;
                    type = EnemyType.Slow;
                    hasEnemiesToSpawn = false; 
                }
                else if (random == 1 && enemies2 > 0)
                {
                    enemies2--;
                    type = EnemyType.Fast;
                    hasEnemiesToSpawn = false;
                }
                else
                {
                    enemies3--;
                    type = EnemyType.Money;
                    hasEnemiesToSpawn = false;
                }

            } while (hasEnemiesToSpawn);

            SpawnEnemy(type);
        }

        public void AddEnemy(GameObject enemy)
        {
            enemiesList.Add(enemy);
        }

        public void RemoveEnemy(GameObject enemy, int gold, int points)
        {
            if (enemiesList.Contains(enemy))
            {
                enemiesKilled++;
                enemiesList.Remove(enemy);
                uiManager.SetEnemiesLeftText(numberOfEnemies - enemiesKilled);
                playerManager.AddPoints(points);
                playerManager.AddEnemiesKilled();
                playerEconomyManager.AddGoldCurrency(gold);
            }
        }

        private void CheckForEndLevel()
        {
            if (levelStarted && enemiesKilled == numberOfEnemies)
            {
                gameManager.WaveCleared();
                isBossLevel = false;
                levelStarted = false;
                isRandomSpawn = false;
            }
        }

        private void EnableSpawn() => canSpawn = true;
    }
}