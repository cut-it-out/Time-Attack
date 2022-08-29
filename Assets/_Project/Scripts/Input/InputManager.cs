using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TimeAttack
{
    public class InputManager : Singleton<InputManager>
    {
        #region Events

        public delegate void Movement(Vector2 direction);
        public event Movement OnMovement;
        public delegate void PauseMenuToggle();
        public event PauseMenuToggle OnPauseMenuToggle;
        public delegate void AnyKeyPress();
        public event AnyKeyPress OnAnyKeyPressed;
        public delegate void SpaceKeyPress();
        public event SpaceKeyPress OnSpaceKeyPressed;

        #endregion

        // cached variables
        private GameInput gameInput;

        protected override void Awake()
        {
            base.Awake();
            gameInput = new GameInput();
        }

        private void OnEnable()
        {
            gameInput.Enable();
        }

        private void OnDisable()
        {
            gameInput?.Disable();
        }

        void Start()
        {
            gameInput.GamePlay.Movement.started += ctx => OnWASDPressed(ctx);
            gameInput.GamePlay.Movement.canceled += ctx => OnWASDPressed(ctx);
            gameInput.GamePlay.PauseMenuActivation.started += ctx => PauseMenuKeyPressed(ctx);
            gameInput.GamePlay.AnyKeyPress.started += ctx => AnyKeyPressed(ctx);
            gameInput.GamePlay.StartGameWithSpace.started += ctx => SpaceKeyPressed(ctx);
        }

        private void AnyKeyPressed(InputAction.CallbackContext ctx)
        {
            OnAnyKeyPressed?.Invoke();
        }

        private void SpaceKeyPressed(InputAction.CallbackContext ctx)
        {
            OnSpaceKeyPressed?.Invoke();
        }

        private void PauseMenuKeyPressed(InputAction.CallbackContext ctx)
        {
            OnPauseMenuToggle?.Invoke();
        }

        private void OnWASDPressed(InputAction.CallbackContext context)
        {
            //OnMovement?.Invoke(context.ReadValue<Vector2>().normalized);
            OnMovement?.Invoke(context.ReadValue<Vector2>());
        }

    }
}
