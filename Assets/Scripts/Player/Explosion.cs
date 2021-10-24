using UnityEngine;

namespace Clear
{
    public class Explosion : MonoBehaviour
    {
        [Header("Attributes.")]
        [SerializeField]
        private float radius;
        [SerializeField]
        private int damage;
        [SerializeField]
        private float openTime = 0.2f;

        private void OnEnable()            
        {
            RaycastHit[] colliders = Physics.BoxCastAll(transform.position, Vector3.one * radius, Vector3.up);
            foreach (RaycastHit hit in colliders)
            {
                TakeDamage target = hit.transform.GetComponent<TakeDamage>();
                if (target != null)
                    target.TakeDamage(damage);
            }


            LeanTween.scale(gameObject, Vector3.one, openTime).setEase(LeanTweenType.animationCurve);
            Destroy(gameObject, openTime);
        }
    }
}