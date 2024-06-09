using UnityEngine;

namespace Unrez.Pets
{
    [CreateAssetMenu(fileName = "New CatProfile", menuName = "Unrez/Cats/New Cat", order = 0)]
    public class PetProfile : ScriptableObject
    {
        public PetType PetType;
        public PetLightProfile Light;
        public Color Color;
        public float SpeedSprint;
        public float Acceleration;
        public float Deceleration;
        public float DashForce;
        public float DashCooldown;
        public float DashDuration;
        public float HairballCooldown;
    }
}