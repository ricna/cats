using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

namespace Unrez.BackyardShowdown
{
    [CreateAssetMenu(fileName = "New Input Reader", menuName = "Unrez/Input Reader")]
    public class InputReader : ScriptableObject, IGameplayActions
    {
        private Controls controls;

        public event Action<Vector2> OnMoveEvent;
        public event Action<bool> OnSprintEvent;
        public event Action<bool> OnCrouchEvent;
        public event Action<int> OnAbilityEvent;
        public event Action<bool> OnInteractEvent;
        public event Action OnToggleMenuEvent;
        public event Action OnMinimapEvent;
        public event Action<bool> OnFollowMouseEvent;
        public event Action<Vector2> OnMousePositionEvent;


        private void OnEnable()
        {
            if (controls == null)
            {
                controls = new Controls();
                controls.Gameplay.SetCallbacks(this);
            }
            controls.Gameplay.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            //Vector2 input = context.ReadValue<Vector2>();
            OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAbility01(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbilityEvent?.Invoke(0);
            }
        }

        public void OnAbility02(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbilityEvent?.Invoke(1);
            }
        }

        public void OnAbility03(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbilityEvent?.Invoke(2);
            }
        }

        public void OnAbility04(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAbilityEvent?.Invoke(3);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnInteractEvent?.Invoke(true);
            }
            else if (context.canceled)
            {
                OnInteractEvent?.Invoke(false);
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnSprintEvent?.Invoke(true);
            }
            else if (context.canceled)
            {
                OnSprintEvent?.Invoke(false);
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnCrouchEvent?.Invoke(true);
            }
            else if (context.canceled)
            {
                OnCrouchEvent?.Invoke(false);
            }
        }

        public void OnMinimap(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnMinimapEvent?.Invoke();
            }
        }

        public void OnToggleMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnToggleMenuEvent?.Invoke();
            }
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            OnMousePositionEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAction01(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnFollowMouseEvent?.Invoke(true);
            }
            else if (context.canceled)
            {
                OnFollowMouseEvent?.Invoke(false);
            }
        }

        public void OnAction02(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }
    }
}