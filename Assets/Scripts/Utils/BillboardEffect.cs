using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clear.Managers;

namespace Clear.Utils
{
    public class BillboardEffect : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(transform.position + GameManager.GetInstance().CurrentCamera.transform.forward);
        }
    }
}