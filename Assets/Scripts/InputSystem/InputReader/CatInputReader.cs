using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static CatControls;

namespace Unrez.Cats
{
    [CreateAssetMenu(fileName = "New Input Reader", menuName = "Unrez/Input Reader")]
    public class CatInputReader : ScriptableObject, ICatActions
    {
        private CatControls controls;

        public event Action<Vector2> OnMoveEvent;
        public event Action<Vector2> OnAimEvent;
        public event Action OnBarrierEvent;
        public event Action OnDashEvent;

        private void OnEnable()
        {
            if (controls == null)
            {
                controls = new CatControls();
                controls.Cat.SetCallbacks(this);
            }
            controls.Cat.Enable();
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