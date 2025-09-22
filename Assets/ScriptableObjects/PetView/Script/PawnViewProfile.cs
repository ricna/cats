using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.BackyardShowdown
{
    [CreateAssetMenu(fileName = "New Pet View", menuName = "Unrez/Cats/Pet View", order = 0)]
    public class PawnViewProfile : ScriptableObject
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

        [Header("FOV (OrtographicSize)")]
        public float OrthoSize = 16;
    }
}
