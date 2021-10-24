using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clear.Managers;

namespace Clear
{
    public class PlayerGFX : MonoBehaviour
    {
        private PlayerInput playerInput;
        private Animator animator;

        private GameManager gameManager;

        private void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            animator = GetComponentInChildren<Animator>();

            gameManager = GameManager.GetInstance();
        }

        private void Update()
        {
            if (gameManager == null) gameManager = GameManager.GetInstance();

            if (!gameManager.InputEnabled)
            {
                animator.SetBool(GameConstants.WALK_BOOL_NAME, false);
                return;
            }

            animator.SetBool(GameConstants.WALK_BOOL_NAME, (playerInput.MovementInput != Vector2.zero));
        }
    }
}