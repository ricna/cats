using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Netcode;
using UnityEngine;

namespace Unrez
{
    public class ProjectileLauncher : NetworkBehaviour
    {

        [Header("References")]
        [SerializeField]
        private InputReader _inputReader;
        [SerializeField]
        private Transform _spawnPoint;
        [SerializeField]
        private GameObject _prefabProjectileClient;
        [SerializeField]
        private GameObject _prefabProjectileServer;
        [SerializeField]
        private Collider2D _collider;
        [SerializeField]
        private GameObject _muzzleFlash;
        [SerializeField]
        private AudioClip _audioFire;

        [Header("Settings")]
        [SerializeField]
        private float _speed;
        [SerializeField]
        private bool _isFiring;
        [SerializeField]
        private float _fireRate;
        [SerializeField]
        private float _muzzleFlashDuration;

        private Coroutine _coroutineFiring;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnFireEvent += Fire;
        }

        public override void OnNetworkDespawn()
        {
            if (!IsOwner)
            {
                return;
            }
            _inputReader.OnFireEvent -= Fire;

        }

        private void Fire(bool firing)
        {
            _isFiring = firing;
            if (_coroutineFiring != null)
            {
                StopCoroutine(_coroutineFiring);
            }
            if (_isFiring)
            {
                _coroutineFiring = StartCoroutine(Firing());
            }
        }

        private IEnumerator Firing()
        {
            while (_isFiring)
            {
                SpawnProjectile();
                SpawnProjectileServerRpc();
                yield return new WaitForSeconds(_fireRate);
            }
        }

        private void SpawnProjectile()
        {
            ProjectileBase projectile = Instantiate(_prefabProjectileClient, _spawnPoint.position, _spawnPoint.rotation, null).GetComponent<ProjectileBase>();
            Physics2D.IgnoreCollision(_collider, projectile.GetComponent<Collider2D>());
            projectile.SetSpeed(_speed);
            projectile.Fire();
            AudioManager.Instance.Play(_audioFire);
            StartCoroutine(MuzzeFlashEffect());
        }

        private IEnumerator MuzzeFlashEffect()
        {
            _muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(_muzzleFlashDuration);
            _muzzleFlash.SetActive(false);
        }

        [ServerRpc]
        private void SpawnProjectileServerRpc()
        {
            ProjectileBase projectile = Instantiate(_prefabProjectileServer, _spawnPoint.position, _spawnPoint.rotation, null).GetComponent<ProjectileBase>();
            Physics2D.IgnoreCollision(_collider, projectile.GetComponent<Collider2D>());
            projectile.SetSpeed(_speed);
            projectile.Fire();
            projectile.SetOwner(OwnerClientId);
            Unbug.Log($"Spawn SERVER RPC {OwnerClientId}");
            SpawnProjectileClientRpc();
        }

        [ClientRpc]
        private void SpawnProjectileClientRpc()
        {
            if (IsOwner)
            {
                return;
            }
            SpawnProjectile();
        }
    }
}