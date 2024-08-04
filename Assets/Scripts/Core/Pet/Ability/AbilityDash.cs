using System.Collections;
using UnityEngine;

namespace Unrez.Backyard.Abilities
{
    public class AbilityDash : Ability
    {
        [Header("Settings - Dash")]
        [SerializeField]
        private SpriteRenderer _spriteRenderBody;
        [SerializeField]
        protected float _dashForce = 30;

        protected override void Prepare()
        {
            Debug.Log("Dash Prepared");
            _spriteRenderBody = _pet.GetComponent<SpriteRenderer>();
        }

        protected override IEnumerator Executing()
        {
            Debug.Log("Dash - IEnumerator Executing();");
            _spriteRenderBody.color = _pet.GetStatus().Color;
            _pet.ApplyImpulse(_dashForce);
            yield return new WaitForSeconds(_abilityDuration);
            _isExecuting = false;
        }

        protected override void Ready()
        {
            _spriteRenderBody.color = _pet.GetStatus().Color;
        }
    }
}
