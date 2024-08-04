using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Unrez.Essential.Audio;
using Unrez.Pets;
using Unrez.Pets.Cats;
using Unrez.Pets.Dogs;

namespace Unrez
{
    public class BoneSpot : NetworkBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        [Tooltip("Amount of 'digs' needed (each catDig decrease 1 dig")]
        private float _digs = 90;
        [SerializeField]
        [Tooltip("Rebury ")]
        private float _reburyCost = 0.25f;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [Header("Effects")]
        [SerializeField]
        private ParticleSystem _particleRebury;

        [Header("Audio")]
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _audioClipRebury;
        [SerializeField]
        private AudioClip _audioClipDigging;
        [SerializeField]
        private AudioClip _audioClipDugUp;

        [Header("Debugs")]
        [SerializeField]
        public NetworkVariable<float> _diggingProgrees = new NetworkVariable<float>(); // the % of the progress (using the _elapsingDigs)
        [SerializeField]
        private NetworkVariable<int> _catsDigging = new NetworkVariable<int>();
        [SerializeField]
        private NetworkVariable<float> _elapsingDigs = new NetworkVariable<float>();
        [SerializeField]
        private DigSpot[] _digSpots;

        public event Action<BoneSpot> OnBoneSpotDugUp;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                _catsDigging.Value = 0;
                _elapsingDigs.Value = _digs;
                _diggingProgrees.Value = 100 - (_elapsingDigs.Value / _digs * 100);
                _diggingProgrees.OnValueChanged += CheckProgress;
            }
            _digSpots = GetComponentsInChildren<DigSpot>();
            foreach (DigSpot digSpot in _digSpots)
            {
                digSpot.OnInteracting += OnDigSpotInteracting;
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            foreach (DigSpot digSpot in _digSpots)
            {
                digSpot.OnInteracting -= OnDigSpotInteracting;
            }
        }

        public void Update()
        {
            if (!IsServer)
            {
                return;
            }
            if (_catsDigging.Value > 0)
            {
                _elapsingDigs.Value -= Time.deltaTime;
                UpdateDiggingProgress();
            }
        }

        private void UpdateDiggingProgress()
        {
            _diggingProgrees.Value = 100 - (_elapsingDigs.Value / _digs * 100);
        }

        private bool _isDugUp = false;
        private void CheckProgress(float previousValue, float newValue)
        {
            if (_isDugUp)
            {
                return;
            }
            if (newValue >= 100)
            {
                _isDugUp = true;
                DugUpClientRpc();
            }
        }

        [ClientRpc]
        private void DugUpClientRpc()
        {
            if (!IsOwner)
            {
                return;
            }
            AudioManager.Instance.Play(_audioSource, _audioClipDugUp);
            OnBoneSpotDugUp?.Invoke(this);
        }

        private void OnDigSpotInteracting(bool isCat, bool interacting)
        {
            if (!IsOwner)
            {
                return;
            }
            OnDigSpotInteractingServerRpc(isCat, interacting);
        }

        [ServerRpc]
        private void OnDigSpotInteractingServerRpc(bool isCat, bool interacting)
        {

            if (isCat)
            {
                if (interacting)
                {
                    _catsDigging.Value++;
                }
                else
                {
                    _catsDigging.Value--;
                }
            }
            else
            {
                ReburyServerRpc();
            }
            OnDigSpotInteractingClientRpc(_catsDigging.Value > 0);
        }

        [ClientRpc]
        private void OnDigSpotInteractingClientRpc(bool catsAreDigging)
        {
            if (!IsOwner)
            {
                return;
            }
            if (catsAreDigging)
            {
                _spriteRenderer.color = Color.red;
                AudioManager.Instance.Play(_audioSource, _audioClipDigging, true, true);
            }
            else
            {
                AudioManager.Instance.Stop(_audioSource);
                _spriteRenderer.color = Color.white;
            }
        }

        [ServerRpc]
        public void ReburyServerRpc()
        {
            _elapsingDigs.Value += _reburyCost;
            //_diggingProgrees.Value = Mathf.Clamp(_diggingProgrees.Value + _digs * _reburyCost, 0, _digs);
            UpdateDiggingProgress();
            ReburyEffectsClientRpc();
        }

        [ClientRpc]
        private void ReburyEffectsClientRpc()
        {
            Debug.Log($"ReburyEffects");
            AudioManager.Instance.Play(_audioSource, _audioClipRebury, true);
            _particleRebury.Play();
        }

        public float GetProgress()
        {
            return _diggingProgrees.Value;
        }
    }
}