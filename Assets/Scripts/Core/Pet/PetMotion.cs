﻿using System;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class PetMotion : NetworkBehaviour
    {
        protected Pet _pet;

        [Header("References")]
        [SerializeField]
        protected Transform _transform;
        [SerializeField]
        protected Rigidbody2D _rb;
        [SerializeField]
        protected Animator _animator;

        [Header("Debugs")]
        protected Vector2 _movementInput;
        [SerializeField]
        protected float _force;
        [SerializeField]
        protected PetSide _petSide;
        [SerializeField]
        protected PetSide _lastPetSide;
        [SerializeField]
        protected bool _isMoving;
        [SerializeField]
        protected bool _inputCrouch = false;
        [SerializeField]
        protected bool _isCrouched;
        [SerializeField]
        protected bool _inputSprint = false;
        [SerializeField]
        protected bool _isSprinting;
        protected bool _isMovingVertical = false;
        protected bool _isMovingHorizontal = false;
        [SerializeField]
        protected Vector2 _currentDirection;
        protected Vector2 _lastDirection;

        //Events
        public event Action<Vector2> OnDirectionChangedEvent;
        public event Action<bool> OnSprintChangedEvent;
        public event Action<bool> OnCrouchChangedEvent;

        protected virtual void Awake()
        {
            _pet = GetComponent<Pet>();
        }

        protected virtual void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            if (_pet == null)
            {
                return;
            }
            CheckMotionInputs();
            Animate();
        }

        public virtual bool CanSprint()
        {
            return true;
        }

        public virtual bool CanCrouch()
        {
            return true;
        }

        private void CheckMotionInputs()
        {
            if (!CanSprint() && !CanCrouch())
            {
                _isSprinting = false;
                _isCrouched = false;
                return;
            }

            _isMovingHorizontal = _movementInput.x != 0;
            _isMovingVertical = _movementInput.y != 0;
            _isMoving = _isMovingHorizontal || _isMovingVertical;
            if (_isMoving)
            {
                _rb.linearDamping = _pet.Profile.Acceleration;
                bool notWalking = _inputCrouch || _inputSprint;
                if (notWalking)
                {
                    if (_inputSprint)
                    {
                        if (!_isSprinting)
                        {
                            _isSprinting = true;
                            OnSprintChangedEvent?.Invoke(_isSprinting);
                        }
                        if (_isCrouched)
                        {
                            _isCrouched = false;
                            OnCrouchChangedEvent?.Invoke(_isCrouched);
                        }
                        _force = _pet.Profile.SpeedSprint * _rb.linearDamping;
                    }
                    else if (_inputCrouch)
                    {
                        if (_isSprinting)
                        {
                            _isSprinting = false;
                            OnSprintChangedEvent?.Invoke(_isSprinting);
                        }
                        if (!_isCrouched)
                        {
                            _isCrouched = true;
                            OnCrouchChangedEvent?.Invoke(_isCrouched);
                        }
                        _force = _pet.Profile.SpeedCrouch * _rb.linearDamping;
                    }
                }
                else
                {
                    if (_isCrouched)
                    {
                        _isCrouched = false;
                        OnCrouchChangedEvent?.Invoke(_isCrouched);
                    }
                    if (_isSprinting)
                    {
                        _isSprinting = false;
                        OnSprintChangedEvent?.Invoke(_isSprinting);
                    }
                    _force = _pet.Profile.Speed * _rb.linearDamping; // walking
                }
                //Direction
                _currentDirection = _lastDirection = Vector2.up * _movementInput.y + Vector2.right * _movementInput.x;
                if (_isMovingVertical)
                {
                    _petSide = _movementInput.y > 0 ? PetSide.North : PetSide.South;
                }
                if (_isMovingHorizontal)
                {
                    _petSide = _movementInput.x > 0 ? PetSide.East : PetSide.West;
                    //_pet.Flip(_petSide);
                }
                if (_lastPetSide != _petSide)
                {
                    _lastPetSide = _petSide;
                }
                _pet.Flip(_petSide);
                _rb.AddForce(_force * _currentDirection, ForceMode2D.Force);
                OnDirectionChangedEvent?.Invoke(_currentDirection);
            }
            else
            {
                if (_currentDirection != Vector2.zero)
                {
                    _lastDirection = _currentDirection;
                    _currentDirection = Vector2.zero;
                    _rb.linearDamping = _pet.Profile.Deceleration; //Set Deceleration
                    //_force = _pet.Profile.SpeedSprint * _rb.drag;
                    //_rb.AddForce(Vector2.zero, ForceMode2D.Force);
                    OnDirectionChangedEvent?.Invoke(_currentDirection);
                }

                if (_isSprinting)
                {
                    _isSprinting = false;
                    OnSprintChangedEvent?.Invoke(_isSprinting);
                }

                if (_inputCrouch != _isCrouched)
                {
                    _isCrouched = _inputCrouch;
                    OnCrouchChangedEvent?.Invoke(_isCrouched);
                }
            }
        }

        protected virtual void Animate()
        {
            _animator.SetBool(AnimatorParameter.IS_MOVING, _isMoving);
            _animator.SetBool(AnimatorParameter.IS_CROUCHED, _isCrouched);
            _animator.SetBool(AnimatorParameter.IS_SPRINTING, _isSprinting);
            _animator.SetFloat(AnimatorParameter.PET_SIDE, (int)_lastPetSide);
        }

        public virtual void ApplyImpulse(float impulse, float newLinearDrag = -1, bool useNewDirection = false, float newDirX = 0, float newDirY = 0)
        {
            if (newLinearDrag != -1)
            {
                _rb.linearDamping = 0;
            }
            Vector2 direction;
            if (useNewDirection)
            {
                direction = new Vector2(newDirX, newDirY);
            }
            else
            {
                direction = _lastDirection;
            }
            _rb.AddForce(direction * impulse, ForceMode2D.Impulse);
        }

        public bool IsMoving()
        {
            return _isMoving;
        }

        public bool IsSprinting()
        {
            return _isSprinting;
        }

        public Vector2 GetDirection()
        {
            return _lastDirection;
        }
        public PetSide GetPetSide()
        {
            return _petSide;
        }

        public void SetMovementInput(Vector2 movementInput)
        {
            _movementInput = movementInput;
        }

        public virtual void SetSprintInput(bool sprint)
        {
            if (!CanSprint())
            {
                sprint = false;
            }
            _inputSprint = sprint;
        }

        public virtual void SetCrouchInput(bool crouch)
        {
            if (!CanCrouch())
            {
                crouch = false;
            }
            _inputCrouch = crouch;
        }
    }
}
