using Unity.Netcode;
using UnityEngine;

namespace Unrez
{
    public class DealDamageOnContact : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private int _damage = 5;

        [Header("Debugs")]
        [SerializeField]
        private ulong _myOwnerID;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody == null)
            {
                return;
            }
            Cat player = collision.attachedRigidbody.GetComponent<Cat>();
            if (player == null)
            {
                return;
            }

            Unbug.Log($"collision {collision.name}", Uncolor.Orange);
            if (player.TryGetComponent(out NetworkBehaviour networkBehaviour))
            {
                if (_myOwnerID == networkBehaviour.OwnerClientId)
                {
                    return;
                }
            }
            player.TakeDamage(_damage);
        }

        public void SetOwner(ulong id)
        {
            Unbug.Log($"SetOwner: {id}", Uncolor.Blue);
            _myOwnerID = id;
        }
    }
}
