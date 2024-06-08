using System;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.Cats
{
    public class HealthController : NetworkBehaviour
    {
        [field: SerializeField]
        public int MaxHealth { get; private set; } = 100;

        public NetworkVariable<int> Health;

        [field: SerializeField]
        public NetworkVariable<bool> IsDead { get; private set; }

        public event Action<HealthController> OnDeath;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }
            IsDead.Value = false;
            ModifyHealth(MaxHealth);
        }

        public void TakeDamage(int damage)
        {
            ModifyHealth(-damage);
        }
        public void RestoreHealth(int healValue)
        {
            ModifyHealth(healValue);
        }

        private void ModifyHealth(int value)
        {
            if (IsDead.Value)
            {
                return;
            }
            Health.Value = Mathf.Clamp(Health.Value + value, 0, MaxHealth);
            if (Health.Value <= 0)
            {
                IsDead.Value = true;
                OnDeath?.Invoke(this);
            }
        }
    }
}