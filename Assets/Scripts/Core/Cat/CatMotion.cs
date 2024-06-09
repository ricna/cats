
namespace Unrez.Pets.Cats
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
            if (_cat.IsDashing())
            {
                return;
            }
            base.FixedUpdate();
        }
    }
}