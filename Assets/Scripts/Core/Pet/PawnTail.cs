using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class PawnTail : MonoBehaviour
    {

        private Pawn _pawn;
        [SerializeField]
        private float _tailDistance = 1.5f;
        private Vector2 _auxPetDirection;
        private Vector2 _auxPetCenter;

        private void Start()
        {
            _pawn = GetComponentInParent<Pawn>();
        }


        /*
        private void Update()
        {
            _auxPetDirection = _pet.GetCurrentDirection();
            _auxPetCenter = _pet.GetCenter();
            transform.position = new Vector2(_auxPetCenter.x - _auxPetDirection.x * _tailDistance, _auxPetCenter.y - _auxPetDirection.y * _tailDistance);
        }*/
    }
}