using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

namespace Unrez.Pets.Cats
{
    [CreateAssetMenu(fileName = "New Input Reader", menuName = "Unrez/Input Reader")]
    public class InputReader : ScriptableObject, ICatActions
    {
        private Controls controls;

        public event Action<Vector2> OnMoveEvent;

        public event Action<bool> OnSprintEvent;
        public event Action<bool> OnCrouchEvent;

        public event Action<int> OnAbilityEvent;

        private void OnEnable()
        {
            if (controls == null)
            {
                controls = new Controls();
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

        }

        public void OnAbility01(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbilityEvent?.Invoke(1);
            }
        }

        public void OnAbility02(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbilityEvent?.Invoke(2);
            }
        }

        public void OnAbility03(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbilityEvent?.Invoke(3);
            }
        }

        public void OnAbility04(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbilityEvent?.Invoke(4);
            }
        }
    }
}