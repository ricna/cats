using Unity.Netcode;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class CatEffects : NetworkBehaviour
    {
        [SerializeField]
        private CatMotion _motion;
        [SerializeField]
        private ParticleSystem _psFootsteps;

        private void Awake()
        {
            _motion = GetComponent<CatMotion>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                // Para o dono, ativamos e desativamos o efeito de forma local e via rede.
                _motion.OnSprintChangedEvent += OnSprintChangedHandler;
            }

            // Todos os clientes (incluindo o dono) devem ouvir a direção de rede para a rotação.
            _motion.NetDirection.OnValueChanged += OnDirectionChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            if (IsOwner)
            {
                _motion.OnSprintChangedEvent -= OnSprintChangedHandler;
            }
            _motion.NetDirection.OnValueChanged -= OnDirectionChanged;
        }

        private void OnDirectionChanged(Vector2 oldDirection, Vector2 newDirection)
        {
            if (newDirection != Vector2.zero)
            {
                // Usa a direção de movimento exata para calcular a rotação em tempo real.
                float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
                _psFootsteps.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }

        private void OnSprintChangedHandler(bool enable)
        {
            if (IsOwner)
            {
                // O dono envia o comando para o servidor.
                ToggleDustTrailServerRpc(enable);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void ToggleDustTrailServerRpc(bool enable)
        {
            // O servidor retransmite o comando para todos os clientes.
            ToggleDustTrailClientRpc(enable);
        }

        [ClientRpc]
        private void ToggleDustTrailClientRpc(bool enable)
        {
            // O efeito é ativado/desativado em todos os clientes, incluindo o dono.
            ToggleDustTrail(enable);
        }

        private void ToggleDustTrail(bool enable)
        {
            if (enable)
            {
                if (!_psFootsteps.isPlaying)
                {
                    _psFootsteps.Play();
                }
            }
            else
            {
                if (_psFootsteps.isPlaying)
                {
                    _psFootsteps.Stop();
                }
            }
        }
    }
}