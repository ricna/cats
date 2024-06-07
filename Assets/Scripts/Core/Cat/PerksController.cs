using Unity.Netcode;
using UnityEngine;

namespace Unrez
{

    public class PerksController : NetworkBehaviour 
    {

        private Cat _cat;

        [Header("References")]
        [SerializeField]
        private InputReader _inputReader;

        [SerializeField]
        private float _cooldownDash = 5;
        [SerializeField]
        private float _cooldownBarrier = 5;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                return;
            }
            _cat = GetComponent<Cat>();
            _inputReader.OnDashEvent += OnDashHandler;
            _inputReader.OnBarrierEvent += OnBarrierHandler;
        }
        private void OnDashHandler()
        {
            _cat.Dash();
        }

        private void OnBarrierHandler()
        {
            _cat.CreateBarrier();
        }
    }
}