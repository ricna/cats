using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.BackyardShowdown
{
    public class PetLight : MonoBehaviour
    {
        [SerializeField]
        protected Light2D _light;

        public void SetUp(Pet pet, PetProfile profile, Vector2 _colliderOffset)
        {
            _light.name = $"PetLight [{profile.name}]";
            _light.enabled = true;
            _light.gameObject.transform.SetParent(pet.transform);
            _light.gameObject.transform.localPosition = _colliderOffset;
            _light.lightType = profile.PetView.LightType;
            _light.color = profile.PetView.LightColor;
            _light.pointLightInnerRadius = profile.PetView.LightRadius.x;
            _light.pointLightOuterRadius = profile.PetView.LightRadius.y;
            _light.intensity = profile.PetView.LightIntensity;
            _light.falloffIntensity = profile.PetView.LightFalloffStrenght;
            _light.shadowsEnabled = profile.PetView.Shadows;
            _light.shadowIntensity = profile.PetView.ShadowsStrenght;
            _light.shadowSoftness = profile.PetView.ShadowsSoftness;
            _light.shadowSoftnessFalloffIntensity = profile.PetView.ShadowsFalloffStrenght;
        }
        public void SetOuterRadius(float radius)
        {
            _light.pointLightOuterRadius = radius;
        }
    }
}