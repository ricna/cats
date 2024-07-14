using UnityEngine;
using Unrez.Pets.Abilities;

namespace Unrez.Pets
{
    [CreateAssetMenu(fileName = "New PetProfile", menuName = "Unrez/Cats/Pet", order = 0)]
    public class PetProfile : ScriptableObject
    {
        public PetType PetType;
        public PetViewProfile PetView;
        public Ability[] Abilities;
        public Color Color;
        public float Speed;
        public float SpeedSprint;
        public float SpeedCrouch;
        public float Acceleration;
        public float Deceleration;
        public float DashForce;
        public float DashCooldown;
        public float DashDuration;
        public float HairballCooldown;
    }
}