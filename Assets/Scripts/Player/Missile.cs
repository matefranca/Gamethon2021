using Clear.Managers;
using UnityEngine;

namespace Clear
{
    public class Missile : MonoBehaviour
    {
        private GameObject explosion;

        public void Init(GameObject explosion)
        {
            this.explosion = explosion;
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.transform.tag)
            {
                case GameConstants.ENEMY_TAG:
                case GameConstants.BOSS_TAG:
                case GameConstants.WALL_TAG:
                case GameConstants.FLOOR_TAG:
                    AudioManager.GetInstance().Play(GameConstants.EXPLOSION_SOUND_NAME);
                    Debug.Log("Missile Hit.");
                    Destroy(Instantiate(explosion, other.transform.position, Quaternion.identity), 0.3f);
                    Destroy(gameObject);
                    break;
            }
        }
    }
}