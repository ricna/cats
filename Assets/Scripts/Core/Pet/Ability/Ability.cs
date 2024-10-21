using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public abstract class Ability : NetworkBehaviour
    {
        protected Pet _pet;

        [Header("Settings - Ability")]
        [SerializeField]
        protected float _abilityDuration = 0.25f;
        [SerializeField]
        protected float _abilityCooldown = 3;

        [Header("Debug - Ability")]
        protected bool _canExecute = true;
        protected bool _isExecuting = false;
        protected float _abilityCharge = 1;

        public event Action OnExecutedEvent;
        public event Action OnChargedEvent;

        protected virtual void Awake()
        {
            _pet = GetComponent<Pet>();
            Prepare();
        }

        /// <summary>
        /// To intialize everything needed. GetComponents, etc
        /// </summary>
        protected abstract void Prepare();
        public virtual bool Execute()
        {
            if (!_canExecute)
            {
                return false;
            }
            _canExecute = false;
            _isExecuting = true;
            StartCoroutine(Executing());
            StartCoroutine(WaitForCooldown());
            return true;
        }
        private IEnumerator WaitForCooldown()
        {
            while (_isExecuting)
            {
                yield return null;
            }
            OnExecutedEvent?.Invoke();
            StartCoroutine(Cooldown());
        }
        /// <summary>
        /// Should set _isExecuting = false in the end.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator Executing();

        private IEnumerator Cooldown()
        {
            _isExecuting = false;
            _abilityCharge = 0;
            while (_abilityCharge < 1)
            {
                _abilityCharge += Time.deltaTime / _abilityCooldown;
                yield return null;
            }
            _abilityCharge = 1;
            _canExecute = true;
            OnChargedEvent?.Invoke();
            Ready();
        }

        /// <summary>
        /// Called after Cooldown. This method should reset the Ability behaviour, attributes, etc, to let the Abiltity Ready to be used again.
        /// </summary>
        protected abstract void Ready();

        public virtual float GetDuration()
        {
            return _abilityDuration;
        }
        public virtual float GetCooldown()
        {
            return _abilityCooldown;
        }
        public virtual float GetCharge()
        {
            return _abilityCharge;
        }
        public virtual bool CanExecute()
        {
            return _canExecute;
        }
        public virtual bool IsExecuting()
        {
            return _isExecuting;
        }
    }
}