using UnityEngine;

namespace Unrez.Pets.Dogs
{
    public class Dog : Pet
    {
        public override void OnDigSpotEnter()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnDigSpotExit()
        {
            //throw new System.NotImplementedException();
        }

        public override void ProcessInteractInput(bool pressing)
        {
            //Debug.Log()
        }

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