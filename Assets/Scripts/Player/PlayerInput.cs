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
        private KeyCode secondGunKey = KeyCode.Alpha2;
        [SerializeField]
        private KeyCode thirdGunKey = KeyCode.Alpha3;
        [SerializeField]
        private KeyCode forthGunKey = KeyCode.Alpha4;
        [SerializeField]
        private KeyCode fifthGunKey = KeyCode.Alpha5;

        public Vector2 MovementInput { get; private set; }

        public Vector3 MousePosition { get; private set; }

        public delegate void OnKeycodeInput();
        public OnKeycodeInput onShootInput;

        public delegate void OnGunKeyCodeInput(int index);
        public OnGunKeyCodeInput onGunInput;


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
            if (Input.GetKeyDown(firstGunKey)) onGunInput?.Invoke(0);
            if (Input.GetKeyDown(secondGunKey)) onGunInput?.Invoke(1);
            if (Input.GetKeyDown(thirdGunKey)) onGunInput?.Invoke(2);
            if (Input.GetKeyDown(forthGunKey)) onGunInput?.Invoke(3);
            if (Input.GetKeyDown(fifthGunKey)) onGunInput?.Invoke(4);
        }
    }
}