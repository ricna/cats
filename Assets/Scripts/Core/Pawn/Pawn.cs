using UnityEngine;

namespace Unrez.Pawn
{
    public class Pawn : MonoBehaviour
    {
        [Header("References")]
        protected PawnMovement _movement;
        protected PawnAnimator _animator;
        protected PawnInputHandler _inputHandler;
        private void Start()
        {

        }
    }
}