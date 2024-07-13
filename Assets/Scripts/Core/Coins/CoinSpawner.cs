using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Unrez
{
    public class CoinSpawner : NetworkBehaviour
    {
        [SerializeField]
        private RespawningCoin _prefabCoin;
        [SerializeField]
        private float _respawnDelay = 3;
        [SerializeField]
        private int _amount = 15;
        [SerializeField]
        private int _coinValue = 10;
        [SerializeField]
        private LayerMask _layerMask;
        [SerializeField]
        private Vector2 _spawnRangeX;
        [SerializeField]
        private Vector2 _spawnRangeY;
        private float _coinRadius;

        private Collider2D[] _coinBuffer = new Collider2D[1];

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                return;
            }
            _coinRadius = _prefabCoin.GetComponent<CircleCollider2D>().radius;
            for (int i = 0; i < _amount; i++)
            {
                SpawnCoin();
            }
        }

        private void SpawnCoin()
        {
            RespawningCoin coinInstance = Instantiate(_prefabCoin, GetSpawnPoint(), Quaternion.identity);
            coinInstance.OnCollected += HandleCoinCollected;
            coinInstance.SetValue(_coinValue);
            coinInstance.GetComponent<NetworkObject>().Spawn();
        }

        private void HandleCoinCollected(RespawningCoin respawningCoin)
        {
            StartCoroutine(Respawn());
            IEnumerator Respawn()
            {
                yield return new WaitForSeconds(_respawnDelay);
                respawningCoin.transform.position = GetSpawnPoint();
                respawningCoin.ResetCoin();
            }
        }

        private Vector2 GetSpawnPoint()
        {
            while (true)
            {
                Vector2 spawnPoint = new Vector2(Random.Range(_spawnRangeX.x, _spawnRangeX.y), Random.Range(_spawnRangeY.x, _spawnRangeY.y));
                int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPoint, _coinRadius, _coinBuffer, _layerMask);
                if (numColliders == 0)
                {
                    //Debug.Log($"numColliders {spawnPoint}");
                    return spawnPoint;
                }
            }
        }
    }
}