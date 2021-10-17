using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clear.Managers
{
    public enum EnemyType
    {
        Fast,
        Slow
    }

    public class SpawnerManager : SingletonInstance<SpawnerManager>
    {
        [Header("Spawn Points.")]
        [SerializeField]
        private Transform spawnPointsParent;

        [Header("Enemy Prefab.")]
        [SerializeField]
        private GameObject enemyPrefab;

        [Header("Types of enemys.")]
        [SerializeField]
        private EnemySO[] enemiesScriptables;

        private List<Transform> spawnPointsList;

        private Dictionary<EnemyType, EnemySO> enemiesDictionary;

        private int numberOfEnemies;
        private float timeBtwSpawn;
        private EnemyType currentType;
        private bool canSpawn;
        private bool levelStarted;

        public List<GameObject> enemiesList;

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

            foreach (EnemySO so in enemiesScriptables)
            {
                enemiesDictionary[so.enemyType] = so;
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
        }

        private void Spawn()
        {
            if (gameManager.PlayEnabled && canSpawn)
            {
                numberOfEnemies--;

                SpawnEnemy(currentType);
                canSpawn = false;
                Invoke(enableSpawnFuncName, timeBtwSpawn);

                if (numberOfEnemies <= 0)
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
            GameObject enemy = Instantiate(enemyPrefab, spawnPointsList[rand].position, Quaternion.identity);
            enemiesList.Add(enemy);
            enemy.GetComponent<Enemy>().Init(enemySO);
        }

        public void RemoveEnemy(GameObject enemy)
        {
            if (enemiesList.Contains(enemy))
            {
                enemiesList.Remove(enemy);
                int enemies = Mathf.Max(enemiesList.Count, numberOfEnemies);
                uiManager.SetEnemiesLeftText(enemies);
                playerEconomyManager.AddGoldCurrency(10);
            }
        }

        private void CheckForEndLevel()
        {
            if (levelStarted && enemiesList.Count == 0 && numberOfEnemies == 0)
            {
                gameManager.WaveCleared();
                levelStarted = false;
            }
        }

        private void EnableSpawn() => canSpawn = true;
    }
}