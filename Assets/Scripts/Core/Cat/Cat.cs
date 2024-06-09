using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.Pets.Cats
{
    public class Cat : Pet
    {

        public override void TakeDamage(int damage)
        {
            _healthController.TakeDamage(damage);
        }

        public override void TryAbility01()
        {
            if (_abilityController.CanDash())
            {
                _spriteRenderBody.color = Color.red;
                _abilityController.ApplyDash();
                StartCoroutine(Dashing());
            }
        }

        public override void TryAbility02()
        {
            _abilityController.CreateBarrier();
        }

        public override void TryAbility03()
        {
        }

        public override void TryAbility04()
        {
        }

        private IEnumerator Dashing()
        {
            while (_abilityController.IsDashing())
            {
                yield return null;
            }
            _spriteRenderBody.color = _petStatus.Color;
        }

        public bool IsDashing()
        {
            return _abilityController.IsDashing();
        }
    }
}