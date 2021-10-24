using UnityEngine;

namespace Clear
{
    public class Bullet : MonoBehaviour
    {
        public int damage;
        public void Init(int damage)
        {
            this.damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.transform.tag)
            {
                case GameConstants.ENEMY_TAG:
                case GameConstants.BOSS_TAG:
                    other.GetComponent<TakeDamage>().TakeDamage(damage);
                    Destroy(gameObject);
                    break;

                case GameConstants.WALL_TAG:
                    Destroy(gameObject);
                    break;
            }
        }
    }
}