using System;
using Unrez.Pets.Abilities;

namespace Unrez.Pets.Cats
{
    public class Cat : Pet
    {
        public override void TakeHit(int damage)
        {
            _healthController.TakeDamage(damage);
        }

        public override void TryAbility(int idxAbility)
        {
            if (_abilitiesController.CanUseAbility(idxAbility))
            {
                _abilitiesController.ExecuteAbility(idxAbility);
            }
        }

        public  bool IsExecutingSomeAbility()
        {
            return _abilitiesController.Busy();
        }

    }
}