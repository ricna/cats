using UnityEngine;

namespace Unrez.Cats
{
    [CreateAssetMenu(fileName = "New CatProfile", menuName = "Unrez/Cats/New Cat", order = 0)]
    public class CatProfile : ScriptableObject
    {
        public Color Color;
        public float Speed;
        public float Acceleration;
        public float Deceleration;
        public float DashForce;
        public float DashCooldown;
        public float DashDuration;
        public float HairballCooldown;
    }
}