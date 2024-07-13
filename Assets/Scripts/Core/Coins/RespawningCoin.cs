
using System;

namespace Unrez
{
    public class RespawningCoin : Coin
    {

        public event Action<RespawningCoin> OnCollected;
        public override int Collect()
        {
            Show(false);
            if (!IsServer)
            {
                return 0;
            }
            if (_collected)
            {
                return 0;
            }
            _collected = true;
            OnCollected?.Invoke(this);
            return _coinValue;
        }

        public void ResetCoin()
        {
            Show(true);
            _collected = false;
        }
    }
}


