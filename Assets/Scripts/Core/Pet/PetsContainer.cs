using Unrez.Essential;

namespace Unrez.Pets
{
    public class PetsContainer : Singleton<PetsContainer>
    {
        public PetProfile[] Pets;

    }
}