using Unrez.Pets;

namespace Unrez
{
    public interface IInteractable
    {
        void Interact(Pet pet);
        void Release();
    }
}