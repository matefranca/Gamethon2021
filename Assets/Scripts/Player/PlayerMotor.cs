using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clear.Managers;

namespace Clear
{
    public class PlayerMotor : MonoBehaviour
    {
        [Header("Attributes.")]
        [SerializeField]
        private float moveSpeed;

        private PlayerInput playerInput;
        private Rigidbody rb;

        private Vector3 targetVector;

        private GameManager gameManager;

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            gameManager = GameManager.GetInstance();
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            Rotate();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (!gameManager.InputEnabled)
            {
                rb.MovePosition(transform.position);
                return;
            }

            targetVector = new Vector3(playerInput.MovementInput.x, 0, playerInput.MovementInput.y);
            targetVector = Quaternion.Euler(0, gameManager.CurrentCamera.transform.root.eulerAngles.y, 0) * targetVector;
            Vector3 targetPosition = transform.position + targetVector.normalized * moveSpeed * Time.deltaTime;

            rb.MovePosition(targetPosition);
        }

        private void Rotate()
        {
            Ray ray = gameManager.CurrentCamera.ScreenPointToRay(playerInput.MousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: 300f))
            {
                Vector3 target = hit.point;
                target.y = transform.position.y;
                transform.LookAt(target);
            }
        }
    }
}