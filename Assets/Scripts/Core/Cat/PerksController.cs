using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.Pets.Cats
{
    public class PerksController : NetworkBehaviour
    {

        protected Cat _cat;

        [Header("References")]
        protected Transform _transform;

        [Header("Perk: Dash")]
        [SerializeField]
        protected float _dashForce = 30;
        [SerializeField]
        private float _dashDuration = 0.25f;
        [SerializeField]
        protected float _dashCooldown = 3;

        protected bool _canDash = true;
        protected bool _isDashing = false;
        protected float _dashCharge;

        [Header("Perk: Hairball")]
        [SerializeField]
        protected Hairball _prefabHairball;
        [SerializeField]
        protected Transform _spawnHairball;
        [SerializeField]
        protected float _spawnHairballOffset = 1.5f;
        [SerializeField]
        protected float _hairballCooldown = 1;

        protected bool _canSpitHairball = true;
        protected bool _isSpittingUpHairball = false;
        protected float _hairballCharge;


        protected virtual void Awake()
        {
            _cat = GetComponent<Cat>();
            _transform = GetComponent<Transform>();
        }

        public virtual void ApplyDash()
        {
            if (!_canDash)
            {
                return;
            }
            StartCoroutine(Dashing());

        }

        protected IEnumerator Dashing()
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

        public virtual void CreateBarrier()
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
            barrier.SetOwnerClientRpc(_cat.GetStatus().OwnerId, _cat.GetStatus().Color);
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