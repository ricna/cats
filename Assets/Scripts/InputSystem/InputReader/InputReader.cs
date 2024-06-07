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

        public event Action<Vector2> OnMoveEvent;
        public event Action<Vector2> OnAimEvent;
        public event Action OnBarrierEvent;
        public event Action OnDashEvent;

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

        public void OnBarrier(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnBarrierEvent?.Invoke();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnDashEvent?.Invoke();
            }
        }
    }
}