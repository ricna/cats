using System;
using Unity.Netcode;
using UnityEngine;
using Unrez.BackyardShowdown;

namespace Unrez.BackyardShowdown
{
    public class PawnAbilities : NetworkBehaviour
    {
        private Pawn _pawn;
        [Header("References")]
        [SerializeField]
        private Ability[] _abilities;

        private void Awake()
        {
            _pawn = GetComponent<Pawn>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
        }

        public bool CanUseAbility(int idxAbility)
        {
            if (_abilities.Length <= idxAbility)
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
                if (ability == null)
                {
                    continue;
                }
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
                if (ability == null)
                {
                    continue;
                }
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
                if (ability == null)
                {
                    continue;
                }
                if (ability.GetType().Equals(abilityType))
                {
                    return ability;
                }
            }
            return null;
        }

        public void SetAbility(int idx, Ability ability)
        {
            if (_abilities.Length == 0)
            {
                Debug.LogError("Should Allocate first!");
                Allocate(4);
            }


            //_abilities[idx] = Instantiate(ability);
            SetupAbilityServerRpc(idx, ability.name);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetupAbilityServerRpc(int idx, string abilityName)
        {
            Debug.Log($"<color=red>SetAbility ServerRpc OwnerClientId{OwnerClientId}</color>");
            /*if (!IsOwner)
            {
                return;
            }*/

            SetupAbilityClientRpc(idx, abilityName);
        }

        private const string AbilityDashName = "AbilityDash";
        private const string AbilityHairball = "AbilityHairball";
        [ClientRpc]
        private void SetupAbilityClientRpc(int idx, string abilityName)
        {
            Ability newAbility = null;
            switch (abilityName)
            {
                case AbilityDashName:
                    newAbility = _pawn.gameObject.AddComponent<AbilityDash>();
                    break;
                case AbilityHairball:
                    newAbility = _pawn.gameObject.AddComponent<AbilityHairball>();
                    break;
            }
            _abilities[idx] = newAbility;
            //_abilities[idx].transform.SetParent(this.transform);
            //Destroy(_abilities[idx].GetComponent<NetworkObject>());
        }

        public void Allocate(int length)
        {
            _abilities = new Ability[length];
        }
    }
}