using Clear.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clear
{
    public class PortalSpawner : MonoBehaviour
    {
        [Header("Attributes.")]
        [SerializeField]
        private float openTime;

        private GameObject enemy;

        public void Init(GameObject enemyToSpawn, EnemySO enemySO)
        {
            Vector3 playerPos = GameManager.GetInstance().PlayerTransform.position;
            transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));

            transform.localScale = Vector3.zero;

            var seq = LeanTween.sequence();
            seq.append(LeanTween.scale(gameObject, Vector3.one, openTime).setEase(LeanTweenType.animationCurve));
            seq.append(() => enemy = Instantiate(enemyToSpawn, transform.position, transform.rotation));
            seq.append(() => enemy.GetComponent<Enemy>().Init(enemySO));
            seq.append(() => SpawnerManager.GetInstance().AddEnemy(enemy));
            seq.append(LeanTween.scale(gameObject, Vector3.zero, openTime).setEase(LeanTweenType.animationCurve));
        }
    }
}