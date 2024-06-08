using Unity.Netcode;
using UnityEngine;

namespace Unrez.Pawn
{
    public class PawnMovement: NetworkBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform _transform;
        [SerializeField]
        private Rigidbody2D _rb;
    }
}