using System;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class PetHealth : NetworkBehaviour
    {
        private int _maxHealth = 100;
        private NetworkVariable<int> _hp = new NetworkVariable<int>(100);
        private NetworkVariable<bool> _isDead = new NetworkVariable<bool>(false);

        [Header("Debugs")]
        [SerializeField]
        private bool _isTakingHit;

        public event Action<PetHealth> OnDeath;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }
            _isDead.Value = false;
            ModifyHealth(_maxHealth);
        }

        public virtual void TakeDamage(int damage)
        {
            ModifyHealth(-damage);
        }

        public virtual void RestoreHealth(int healValue)
        {
            ModifyHealth(healValue);
        }

        protected virtual void ModifyHealth(int value)
        {
            if (_isDead.Value)
            {
                return;
            }
            _hp.Value = Mathf.Clamp(_hp.Value + value, 0, _maxHealth);
            if (_hp.Value <= 0)
            {
                _isDead.Value = true;
                OnDeath?.Invoke(this);
            }
        }

        public virtual bool IsTakingHit()
        {
            return false;
        }
    }
}