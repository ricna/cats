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

        [Header("Camera")]
        public float OrthographicSize = 16;
        public float ScareZoom = 16;
        public float DogBuryingBoneZoom = 8;

        [Header("FOV (x,y) (OrtographicSize,LightRadius.Outer)")]
        [Tooltip("The  and ")]
        public float CatView = 16;
        public float DogView = 24;
        [Header("FOV In Chase")]
        public float CatChaseView = 12;
        public float DogInChase = 16;
        [Header("FOV In Panic")]
        public float CatPanicView = 8;
        public float CatTreeView = 36;

    }
}
