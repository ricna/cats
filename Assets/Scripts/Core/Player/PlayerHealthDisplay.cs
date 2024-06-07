using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Unrez
{

    public class PlayerHealthDisplay : NetworkBehaviour
    {
        [SerializeField]
        private HealthController _playerHealth;
        [SerializeField]
        private Image _imgHealth;

        public override void OnNetworkSpawn()
        {
            if (!IsClient)
            {
                return;
            }
            _playerHealth.Health.OnValueChanged += OnHealthChanged;
            OnHealthChanged(0, _playerHealth.Health.Value);
        }
        public override void OnNetworkDespawn()
        {
            if (!IsClient)
            {
                return;
            }
            _playerHealth.Health.OnValueChanged -= OnHealthChanged;

        }
        private void OnHealthChanged(int oldHealth, int newHealth)
        {
            float health = newHealth / 100f;

            if (health < 0.2)
            {
                _imgHealth.color = Color.red;
            }
            else if (health < 0.5)
            {
                _imgHealth.color = Color.yellow;
            }
            else
            {
                _imgHealth.color = Color.green;
            }
            _imgHealth.fillAmount = health;
        }
    }
}