
using UnityEngine;

namespace Unrez.Pets.Cats
{
    public class CatMotion : PetMotion
    {
        private Cat _cat;

        protected override void Awake()
        {
            base.Awake();
            _cat = GetComponent<Cat>();
        }

        protected override void FixedUpdate()
        {
            if (_cat.IsExecutingSomeAbility())
            {
                return;
            }
            if (_cat.IsDigging())
            {

                //_rb.velocity = Vector2.zero;
                //_currentDirection = Vector2.zero;
                //_movementInput = Vector2.zero;
                return;
            }
            base.FixedUpdate();
        }
    }
}