using UnityEngine;
using Clear.Managers;

namespace Clear
{
    public class DeathCube : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            switch (collision.transform.tag)
            {
                case GameConstants.PLAYER_TAG:
                    PlayerManager.GetInstance().TakeDamage(5000);
                    break;

                case GameConstants.ENEMY_TAG:
                    SpawnerManager.GetInstance().RemoveEnemy(collision.gameObject, 0, 0);
                    break;
            }
        }
    }
}