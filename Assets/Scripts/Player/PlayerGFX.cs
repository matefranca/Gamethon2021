using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Clear
{
    public class PlayerGFX : MonoBehaviour
    {
        private PlayerInput playerInput;
        private Animator animator;

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            animator.SetBool(GameConstants.WALK_BOOL_NAME, (playerInput.MovementInput != Vector2.zero));
        }
    }
}