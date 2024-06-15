using System;
using Unity.Netcode;
using UnityEngine;
using Unrez.Pets;
using Unrez.Pets.Cats;
using Unrez.Pets.Dogs;

namespace Unrez
{
    public class BoneSpot : NetworkBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        [Tooltip("Amount of 'digs' needed (each catDig decrease 1 dig. Each dogRebury will '_reburyCost%' of this value")]
        private float _digs = 90;
        [SerializeField]
        private float _reburyCost = 0.25f;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [Header("Debugs")]
        [SerializeField]
        public NetworkVariable<float> _progress = new NetworkVariable<float>(); // the % of the progress (using the _elapsingDigs)
        [SerializeField]
        private int _catsDigging = 0;
        [SerializeField]
        private float _elapsingDigs = 90;
        [SerializeField]
        private DigSpot[] _digSpots;

        public event Action<BoneSpot> OnBoneSpotDigged;


        private void Awake()
        {
            _elapsingDigs = _digs;
            _digSpots = GetComponentsInChildren<DigSpot>();
            foreach (DigSpot digSpot in _digSpots)
            {
                digSpot.OnInteracting += OnDigSpotInteracting;
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                _progress.Value = 0f;
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
            if (_catsDigging > 0)
            {
                _elapsingDigs -= Time.deltaTime;
                _progress.Value = _elapsingDigs / _digs * 100;
            }
        }

        private void OnDigSpotInteracting(Pet pet, BoneSpot spot, bool interacting)
        {
            if (pet is Cat)
            {
                if (interacting)
                {
                    _catsDigging++;
                }
                else
                {
                    _catsDigging--;
                }
                //Debug.Log($"OnDigSpotInteracting -> _catsDigging:{_catsDigging}");
                if (_catsDigging > 0)
                {
                    _spriteRenderer.color = Color.red;
                }
                else
                {
                    _spriteRenderer.color = Color.white;
                }
            }
            else
            {
                if (pet is Dog)
                {

                }
            }
        }

        public void Rebury()
        {
            _progress.Value = Mathf.Clamp(_progress.Value + _digs * _reburyCost, 0, _digs);
            ReburyEffects();
        }

        private void ReburyEffects()
        {
            Debug.Log($"ReburyEffects");
        }

        public float GetProgress()
        {
            return _progress.Value;
        }
    }
}