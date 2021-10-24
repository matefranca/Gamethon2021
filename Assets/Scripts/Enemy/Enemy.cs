using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Clear.Managers;
using Clear.Utils;

namespace Clear
{
    public class Enemy : MonoBehaviour, TakeDamage
    {
        [Header("Health Bar")]
        [SerializeField]
        private Image healthBar;

        [Header("Gold Effect.")]
        [SerializeField]
        private GameObject goldTextEffect;

        [Header("Particle System.")]
        [SerializeField]
        private ParticleSystem particleSystem;
        
        [Header("Navmesh Agent")]
        [SerializeField]
        private NavMeshAgent agent;

        private Transform target;

        private int goldPoints;
        private int killPoints;
        private float maxLifes;
        private float lifes;
        private int damage;

        private EnemyType enemyType;

        private void Start()
        {
            target = GameManager.GetInstance().PlayerTransform;
            healthBar.fillAmount = 1;
            lifes = maxLifes;
        }

        private void Update()
        {
            Move();
        }

        public void Init(EnemySO enemySO)
        {
            damage = enemySO.damage;
            maxLifes = enemySO.lifes;
            enemyType = enemySO.enemyType;
            agent.speed = enemySO.moveSpeed;
            killPoints = enemySO.killPoints;
            goldPoints = enemySO.goldPoints;
        }

        private void Move()
        {
            if (target == null) return;

            Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

            transform.LookAt(targetPosition);
            agent.SetDestination(targetPosition);
        }

        public void TakeDamage(int amount)
        {
            lifes -= amount;
            healthBar.fillAmount = lifes / maxLifes;

            if (lifes <= 0)
            {
                GameObject goldEffect = Instantiate(goldTextEffect, transform.position, Quaternion.identity);
                goldEffect.GetComponent<GoldBonusText>().Init(goldPoints.ToString());
                SpawnerManager.GetInstance().RemoveEnemy(gameObject, goldPoints, killPoints);
                Destroy(goldEffect, 2f);

                ParticleSystem particle = Instantiate(particleSystem, transform.position, Quaternion.identity);
                particle.Play();
                Destroy(gameObject); 
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag(GameConstants.PLAYER_TAG))
            {
                if (enemyType == EnemyType.Money)
                {
                    GameManager.GetInstance().TakeMoney();
                    CameraController.GetInstance().Shake();
                    PlayerManager.GetInstance().ResetEnemiesKilled();
                }
                else
                {
                    PlayerManager.GetInstance().TakeDamage(damage);
                }
                SpawnerManager.GetInstance().RemoveEnemy(gameObject, 0, 0);
                Destroy(gameObject);
            }
        }
    }
}