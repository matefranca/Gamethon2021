using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clear.Utils
{
    public class CameraController : SingletonInstance<CameraController>
    {
        [Header("Attributes.")]
        [SerializeField]
        private Transform target;
        [SerializeField]
        private Vector3 offset;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            transform.position = target.position + offset;
        }

        public void Shake()
        {
            animator.SetTrigger(GameConstants.SHAKE_TRIGGER);
        }
    }
}