using System.Collections;
using System.Collections.Generic;
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
            if (other.CompareTag(GameConstants.ENEMY_TAG))
            {
                other.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
            }

            if (other.CompareTag(GameConstants.WALL_TAG))
            {
                Destroy(gameObject);
            }
        }
    }
}