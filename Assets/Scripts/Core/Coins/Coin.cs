using Unity.Netcode;
using UnityEngine;

namespace Unrez
{
    public abstract class Coin : NetworkBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        protected int _coinValue = 10;
        [SerializeField]
        protected bool _collected = false;

        public abstract int Collect();
        public void SetValue(int value)
        {
            _coinValue = value;
        }
        protected void Show(bool show)
        {
            _spriteRenderer.enabled = show;
        }
    }
}
