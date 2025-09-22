using UnityEngine;

namespace Unrez.BackyardShowdown
{
    [CreateAssetMenu(fileName = "New PetProfile", menuName = "Unrez/Cats/Pet", order = 0)]
    public class PawnProfile : ScriptableObject
    {
        public PawnType PetType;
        public PawnViewProfile PetView;
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

        public Ability[] Abilities;
    }
}