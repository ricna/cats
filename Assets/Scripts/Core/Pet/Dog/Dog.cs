using UnityEngine;

namespace Unrez.Backyard.Dogs
{
    public class Dog : Pet
    {

        public override void ProcessInteractInput(bool pressing)
        {
        }

        public override void TakeHit(int damage)
        {
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