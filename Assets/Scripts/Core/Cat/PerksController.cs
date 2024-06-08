using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.Cats
{
    public class PerksController : NetworkBehaviour
    {

        private Cat _cat;

        [Header("References")]
        private Transform _transform;

        [Header("Perk: Dash")]
        [SerializeField]
        private float _dashForce = 30;
        [SerializeField]
        private float _dashDuration = 0.25f;
        [SerializeField]
        private float _dashCooldown = 3;

        private bool _canDash = true;
        private bool _isDashing = false;
        private float _dashCharge;

        [Header("Perk: Hairball")]
        [SerializeField]
        private Hairball _prefabHairball;
        [SerializeField]
        private Transform _spawnHairball;
        [SerializeField]
        private float _spawnHairballOffset = 1.5f;
        [SerializeField]
        private float _hairballCooldown = 1;

        private bool _canSpitHairball = true;
        private bool _isSpittingUpHairball = false;
        private float _hairballCharge;


        private void Awake()
        {
            _cat = GetComponent<Cat>();
            _transform = GetComponent<Transform>();
        }

        public void ApplyDash()
        {
            if (!_canDash)
            {
                return;
            }
            StartCoroutine(Dashing());

        }

        private IEnumerator Dashing()
        {
            _canDash = false;
            _isDashing = true;
            _cat.ApplyImpulse(_dashForce);
            yield return new WaitForSeconds(_dashDuration);
            _isDashing = false;
            //cooldown
            _dashCharge = 0;
            while (_dashCharge < 1)
            {
                _dashCharge += Time.deltaTime / _dashCooldown;
                yield return null;
            }
            _dashCharge = 1;
            _canDash = true;
        }

        public void CreateBarrier()
        {
            if (!_canSpitHairball)
            {
                return;
            }
            StartCoroutine(CreatingBarrier());

        }

        private IEnumerator CreatingBarrier()
        {
            _canSpitHairball = false;
            _isSpittingUpHairball = true;
            InstantiateBarrierServerRpc(_spawnHairball.position);
            _isSpittingUpHairball = false;
            _hairballCharge = 0;
            while (_hairballCharge < 1)
            {
                _hairballCharge += Time.deltaTime / _hairballCooldown;
                yield return null;
            }
            _hairballCharge = 1;
            _canSpitHairball = true;
        }

        [ServerRpc]
        private void InstantiateBarrierServerRpc(Vector2 spawn)
        {
            Debug.Log($"CreateBarrierServerRpc IsServer:{IsServer}");
            Hairball barrier = Instantiate(_prefabHairball, spawn, Quaternion.identity);
            barrier.GetComponent<NetworkObject>().Spawn();
            barrier.SetOwnerClientRpc(_cat.GetData().OwnerId, _cat.GetData().Color);
        }

        public void UpdateSpawnBehindPosition(Vector2 catDirection)
        {
            _spawnHairball.position = new Vector2(_transform.position.x - catDirection.x * _spawnHairballOffset, _transform.position.y - catDirection.y * _spawnHairballOffset);
        }

        public bool IsDashing()
        {
            return _isDashing;
        }

        internal bool CanDash()
        {
            return _canDash;
        }
    }
}