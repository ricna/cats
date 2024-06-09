using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static CatControls;

namespace Unrez.Pets.Cats
{
    [CreateAssetMenu(fileName = "New Input Reader", menuName = "Unrez/Input Reader")]
    public class CatInputReader : ScriptableObject, ICatActions
    {
        private CatControls controls;

        public event Action<Vector2> OnMoveEvent;

        public event Action<bool> OnSprintEvent;
        public event Action<bool> OnCrouchEvent;

        public event Action OnAbility01Event;
        public event Action OnAbility02Event;
        public event Action OnAbility03Event;
        public event Action OnAbility04Event;

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

        }

        public void OnAbility01(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbility01Event?.Invoke();
            }
        }

        public void OnAbility02(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbility02Event?.Invoke();
            }
        }

        public void OnAbility03(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbility03Event?.Invoke();
            }
        }

        public void OnAbility04(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbility04Event?.Invoke();
            }
        }
    }
}