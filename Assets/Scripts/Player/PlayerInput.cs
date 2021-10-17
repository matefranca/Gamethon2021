using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Clear.Managers;

namespace Clear
{
    public class PlayerInput : MonoBehaviour
    {
        [Header("Hotkeys.")]
        [SerializeField]
        private KeyCode firstGunKey = KeyCode.Alpha1;
        [SerializeField]
        private KeyCode secondGunKey = KeyCode.Alpha0;

        public Vector2 MovementInput { get; private set; }

        public Vector3 MousePosition { get; private set; }

        public delegate void OnKeycodeInput();
        public OnKeycodeInput onShootInput;
        public OnKeycodeInput onFirstGunInput;
        public OnKeycodeInput onSecondGunInput;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = GameManager.GetInstance();
        }

        private void Update()
        {
            if (!gameManager.InputEnabled) return;

            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            MovementInput = new Vector2(horizontalInput, verticalInput);

            MousePosition = Input.mousePosition;

            if (Input.GetMouseButton(0)) onShootInput?.Invoke();
            if (Input.GetKeyDown(firstGunKey)) onFirstGunInput?.Invoke();
            if (Input.GetKeyDown(secondGunKey)) onSecondGunInput?.Invoke();
        }
    }
}