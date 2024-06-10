using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Unrez.Pets.Abilities;

namespace Unrez.Pets
{
    public class PetAbilities : NetworkBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Ability[] _abilities;

        public bool CanUseAbility(int idxAbility)
        {
            if(_abilities.Length <= idxAbility)
            {
                return false;
            }
            bool otherExecuting = Busy();
            if (otherExecuting)
            {
                return false;
            }
            return _abilities[idxAbility].CanExecute();
        }

        public bool ExecuteAbility(int idxAbility)
        {
            return _abilities[idxAbility].Execute();
        }

        public bool IsExecutingAbility(int idxAbility)
        {
            return _abilities[idxAbility].IsExecuting();
        }

        public float GetAbilityCharge(int idxAbility)
        {
            return _abilities[idxAbility].GetCharge();
        }

        /// <summary>
        /// If executing any ability is Busy, so you CAN NOT execute 2 abilities at the same time
        /// </summary>
        /// <returns></returns>
        public bool Busy()
        {
            foreach (var ability in _abilities)
            {
                if (ability.IsExecuting())
                {
                    return true;
                }
            }
            return false;
        }

        public Ability GetAbilityByName(string abilityName)
        {
            foreach (var ability in _abilities)
            {
                if (ability.GetType().Name == abilityName)
                {
                    return ability;
                }
            }
            return null;
        }

        public Ability GetAbilityByType(Type abilityType)
        {
            foreach (var ability in _abilities)
            {
                if (ability.GetType().Equals(abilityType))
                {
                    return ability;
                }
            }
            return null;
        }

        public void SetAbility(int idx, Ability ability)
        {
            _abilities[idx] = Instantiate(ability);
            _abilities[idx].transform.SetParent(this.transform);
            Destroy(_abilities[idx].GetComponent<NetworkObject>());
        }
    }
}