using Unity.Netcode;
using UnityEngine;

namespace Unrez
{
    public class PlayerCoinWallet : NetworkBehaviour
    {
        public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsServer)
            {
                return;
            }
            if (collision.TryGetComponent(out Coin coin))
            {
                TotalCoins.Value += coin.Collect();
            }
        }
    }
}