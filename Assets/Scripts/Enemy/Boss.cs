using UnityEngine;
using Clear.Managers;
using UnityEngine.UI;
using Clear.Utils;

namespace Clear
{
    public class Boss : MonoBehaviour, TakeDamage
    {
        [Header("Attributes.")]
        [SerializeField]
        private int life;
        [SerializeField]
        private float attackTimer;
        [SerializeField]
        private int goldPoints;
        [SerializeField]
        private int killPoints;
        [SerializeField]
        private int damage;

        [Header("Projectile.")]
        [SerializeField]
        private GameObject attackObject;

        [Header("Gold Effect.")]
        [SerializeField]
        private GameObject goldTextEffect;

        [Header("Health Bar")]
        [SerializeField]
        private Image healthBar;

        [Header("Particle System.")]
        [SerializeField]
        private ParticleSystem particleSystem;

        private float timer;
        private float maxTimer;
        private int maxLife;

        private Transform target;

        private bool isGrounded;

        public void Init(int multiplier, Transform target)
        {
            maxLife = life * multiplier;
            life = maxLife;
            this.target = target;
            killPoints *= multiplier;
            goldPoints *= multiplier;
            maxTimer = attackTimer - (multiplier / 10);
            maxTimer = Mathf.Clamp(maxTimer, 0.1f, attackTimer);
            timer = maxTimer;

            AudioManager.GetInstance().Play(GameConstants.BOSS_SOUND_NAME);
        }

        private void Update()
        {
            if (isGrounded) Attack();
        }

        private void LateUpdate()
        {
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        }

        private void Attack()
        {
            if (timer <= 0)
            {
                Vector3 targetPosition = new Vector3(target.position.x, target.position.y - 1f, target.position.z);
                GameObject pawAttack = Instantiate(attackObject, targetPosition, Quaternion.identity);
                pawAttack.GetComponent<PawAttack>().Init(damage);
                timer = maxTimer;
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }

        public void TakeDamage(int ammount)
        {
            life -= ammount;
            healthBar.fillAmount = life / maxLife;
            if (life <= 0)
            {
                GameObject goldEffect = Instantiate(goldTextEffect, transform.position, Quaternion.identity);
                goldEffect.GetComponent<GoldBonusText>().Init("+" + goldPoints.ToString());
                Destroy(goldEffect, 2f);
                SpawnerManager.GetInstance().RemoveEnemy(gameObject, goldPoints, killPoints);

                ParticleSystem particle = Instantiate(particleSystem, transform.position, Quaternion.identity);
                particle.Play();
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(GameConstants.FLOOR_TAG))
            {
                isGrounded = true;
                CameraController.GetInstance().Shake();
            }
        }
    }
}