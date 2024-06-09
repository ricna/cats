using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.Pets
{
    [CreateAssetMenu(fileName = "New Pet Light", menuName = "Unrez/Cats/Pet Light", order = 0)]
    public class PetLightProfile : ScriptableObject
    {
        public Light2D.LightType LightType;
        public Color LightColor = Color.white;
        public Vector2 LightRadius;
        public float LightIntensity;
        [Range(0, 1)]
        public float LightFalloffStrenght;
 
        public bool Shadows;
        [Range(0, 1)] 
        public float ShadowsStrenght;
        [Range(0, 1)] 
        public float ShadowsSoftness;
        [Range(0, 1)] 
        public float ShadowsFalloffStrenght;
    }
}
