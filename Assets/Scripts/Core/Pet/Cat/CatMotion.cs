
namespace Unrez.BackyardShowdown
{
    public class CatMotion : PetMotion
    {
        private Cat _cat;

        protected override void Awake()
        {
            base.Awake();
            _cat = GetComponent<Cat>();
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

        public override void SetCrouchInput(bool crouch)
        {
            if (!CanCrouch())
            {
                crouch = false;
            }
            _inputCrouch = crouch;
        }

        public override void SetSprintInput(bool sprint)
        {
            if (!CanSprint())
            {
                sprint = false;
            }
            _inputSprint = sprint;
        }

    }
}