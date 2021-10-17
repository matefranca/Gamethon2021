using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clear.Managers;
using UnityEngine.UI;

namespace Clear
{
    public class Enemy : MonoBehaviour
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
        
        private Transform target;

        private float moveSpeed;
        private float maxLifes;
        private float lifes;

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
            moveSpeed = enemySO.moveSpeed;
            maxLifes = enemySO.lifes;
        }

        private void Move()
        {
            if (target == null) return;

            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        public void TakeDamage(int amount)
        {
            lifes -= amount;
            healthBar.fillAmount = lifes / maxLifes;

            if (lifes <= 0)
            {
                Destroy(Instantiate(goldTextEffect, transform.position, Quaternion.identity), 2f);
                SpawnerManager.GetInstance().RemoveEnemy(gameObject);

                ParticleSystem particle = Instantiate(particleSystem, transform.position, Quaternion.identity);
                particle.Play();
                Destroy(gameObject); 
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag(GameConstants.PLAYER_TAG))
            {
                GameManager.GetInstance().RestartGame();
            }
        }
    }
}