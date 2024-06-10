using UnityEngine;

namespace Unrez.Pets.Dogs
{
    public class Dog : Pet
    {
        public override void TakeHit(int damage)
        {
            //_healthController.TakeDamage(damage);
        }

        public override void TryAbility(int idxAbility)
        {
            if (_abilitiesController.CanUseAbility(idxAbility))
            {
                _abilitiesController.ExecuteAbility(idxAbility);
            }
        }
    }
}