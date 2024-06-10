using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Unrez.Pets.Abilities
{

    public class AbilityHairball : Ability
    {
        [Header("Settings - Barrier")]
        [SerializeField]
        protected Hairball _prefabHairball;
        [SerializeField]
        protected Transform _spawnHairball;
        [SerializeField]
        protected float _spawnHairballOffset = 1.5f;
        private Transform _transform;

        protected override void Prepare()
        {
            _transform = transform;
        }

        protected override IEnumerator Executing()
        {
            Vector2 direction = _pet.GetCurrentDirection();
            _spawnHairball.position = new Vector2(_transform.position.x - direction.x * _spawnHairballOffset, _transform.position.y - direction.y * _spawnHairballOffset);
            InstantiateHairballServerRpc(_spawnHairball.position);
            yield return new WaitForSeconds(_abilityDuration);
            _isExecuting = false;
        }

        protected override void Ready()
        {
        }

        [ServerRpc]
        private void InstantiateHairballServerRpc(Vector2 spawn)
        {
            Hairball barrier = Instantiate(_prefabHairball, spawn, Quaternion.identity); 
            barrier.GetComponent<NetworkObject>().Spawn();
            barrier.SetOwnerClientRpc(_pet.GetStatus().OwnerId, _pet.GetStatus().Color);
        }
    }
}