
using System;

namespace Unrez.BackyardShowdown
{
    public class PreyMotion : PawnMotion
    {
        private Prey _cat;

        protected override void Awake()
        {
            base.Awake();
            _cat = GetComponent<Prey>();
        }

        protected override void FixedUpdate()
        {
            if (_cat.IsExecutingSomeAbility())
            {
                return;
            }
            if (_cat.IsDigging())
            {
                return;
            }
            base.FixedUpdate();
        }
    }
}