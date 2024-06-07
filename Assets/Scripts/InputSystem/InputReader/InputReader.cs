using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

namespace Unrez
{
    [CreateAssetMenu(fileName = "New Input Reader", menuName = "Unrez/Input Reader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        private Controls controls;
        private Vector2 _lastMousePosition = Vector2.positiveInfinity;

        public event Action<Vector2> OnMoveEvent;
        public event Action<Vector2> OnAimEvent;
        public event Action<bool> OnFireEvent;
        

        private void OnEnable()
        {
            if (controls == null)
            {
                controls = new Controls();
                controls.Player.SetCallbacks(this);
            }
            controls.Player.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            //Vector2 input = context.ReadValue<Vector2>();
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnFireEvent?.Invoke(true);
            }
            else
            {
                OnFireEvent?.Invoke(false);
            }
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (_lastMousePosition != context.ReadValue<Vector2>())
                {
                    _lastMousePosition = context.ReadValue<Vector2>();
                    OnAimEvent?.Invoke(_lastMousePosition);
                }
            }
        }
    }
}