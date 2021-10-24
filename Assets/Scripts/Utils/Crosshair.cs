using UnityEngine;

namespace Clear.UI
{
    public class Crosshair : MonoBehaviour
    {
        void Update()
        {
            transform.position = Input.mousePosition;
        }
    }
}