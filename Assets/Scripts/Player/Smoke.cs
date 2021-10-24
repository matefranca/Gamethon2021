using UnityEngine;

namespace Clear
{
    public class Smoke : MonoBehaviour
    {
        public void Destroy()
        {
            gameObject.SetActive(false);
        }
    }
}