using UnityEngine;

namespace Unrez.Pets.Dogs
{
    public class Dog : Pet
    {
        public override void TakeHit(int damage)
        {
            //_healthController.TakeDamage(damage);
        }

        public override void TryAbility(int abilityId)
        {
            if (_abilitiesController.CanUseAbility(abilityId))
            {
                _abilitiesController.ExecuteAbility(abilityId);
            }
        }
    }
}