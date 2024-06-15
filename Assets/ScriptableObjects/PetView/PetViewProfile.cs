using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.Pets
{
    [CreateAssetMenu(fileName = "New Pet View", menuName = "Unrez/Cats/Pet View", order = 0)]
    public class PetViewProfile : ScriptableObject
    {
        [Header("Light")]
        public Light2D.LightType LightType;
        public Color LightColor = Color.white;
        public Vector2 LightRadius;
        public float LightIntensity = 1;
        [Range(0, 1)]
        public float LightFalloffStrenght;

        [Header("Shadow")]
        public bool Shadows;
        [Range(0, 1)]
        public float ShadowsStrenght;
        [Range(0, 1)]
        public float ShadowsSoftness;
        [Range(0, 1)]
        public float ShadowsFalloffStrenght;

        [Header("FOV (OrtographicSize & LightRadius.Outer)")]
        public float CatView = 16;
        public float DogView = 24;
        [Header("FOV In Chase")]
        public float CatChaseView = 12;
        public float DogChaseView = 16;

        [Header("Non-Shared")]
        public float DogBuryingView = 8;
        public float CatPanicView = 8;
        public float CatTreeView = 36;

    }
}
