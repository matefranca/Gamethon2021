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

        [Header("Enemy Prefab.")]
        [SerializeField]
        private GameObject[] enemiesPrefabs;
        [SerializeField]
        private GameObject bossPrefab;

        [Header("Types of enemys.")]
        [SerializeField]
        private EnemySO[] enemiesScriptables;

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

        private List<GameObject> enemiesList;
        private int enemiesSpawned;
        private int enemiesKilled;

        private UIManager uiManager;
        private GameManager gameManager;
        private PlayerEconomyManager playerEconomyManager;

        private string enableSpawnFuncName = "EnableSpawn";

        private void Start()
        {
            FillSpawnPoints();
            FillEnemiesDictionary();

            uiManager = UIManager.GetInstance();
            gameManager = GameManager.GetInstance();
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

            foreach (EnemySO so in enemiesScriptables)
            {
                enemiesDictionary[so.enemyType] = so;
                enemiesGameObjectsDictionary[so.enemyType] = so.enemyPrefab;
            }
        }

        public void StartLevel()
        {
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
            if (gameManager.PlayEnabled && canSpawn)
            {
                enemiesSpawned++;

                SpawnEnemy(currentType);
                canSpawn = false;
                Invoke(enableSpawnFuncName, timeBtwSpawn);

                if (enemiesSpawned == numberOfEnemies)
                    gameManager.EndWave();
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

        public void AddEnemy(GameObject enemy)
        {
            enemiesList.Add(enemy);
           
        }

        public void RemoveEnemy(GameObject enemy)
        {
            if (enemiesList.Contains(enemy))
            {
                enemiesKilled++;
                enemiesList.Remove(enemy);
                uiManager.SetEnemiesLeftText(numberOfEnemies - enemiesKilled);
                playerEconomyManager.AddGoldCurrency(10);
            }
        }

        private void CheckForEndLevel()
        {
            if (levelStarted && enemiesKilled == numberOfEnemies)
            {
                gameManager.WaveCleared();
                levelStarted = false;
            }
        }

        private void EnableSpawn() => canSpawn = true;
    }
}