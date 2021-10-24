using UnityEngine;
using Clear.Managers;

namespace Clear
{
    public class PawAttack : MonoBehaviour
    {
        [Header("Attributes.")]
        [SerializeField]
        private int damage = 1;

        private bool canDealDamage;

        public void Init(int damage)
        {
            this.damage = damage;
            canDealDamage = true;
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!canDealDamage) return;

            if (other.CompareTag(GameConstants.PLAYER_TAG))
            {
                PlayerManager.GetInstance().TakeDamage(damage);
                canDealDamage = false;
            }
        }
    }
}