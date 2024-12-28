using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.BackyardShowdown
{
    public class PetLight : MonoBehaviour
    {
        [SerializeField]
        protected Light2D _light;
        [SerializeField]
        private PetTail _tail;
        [Header("Debug")]
        [SerializeField]
        protected Pet _pet;
        [SerializeField]
        protected bool _isCat = false;

        public void SetUp(Pet pet, PetProfile profile, Vector2 _colliderOffset)
        {
            _pet = pet;
            _isCat = _pet is Cat;
            _tail = _pet.GetTail();
            if (!_isCat)
            {
                _light.gameObject.transform.SetParent(_tail.transform);
                _light.gameObject.transform.localPosition = Vector2.zero;
            }
            else
            {
                _light.gameObject.transform.SetParent(_pet.transform);
                _light.gameObject.transform.localPosition = _colliderOffset;
            }

            _light.name = $"PetLight [{profile.name}]";
            _light.enabled = true;

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
        public void SetInnerRadius(float radius)
        {
            _light.pointLightInnerRadius = radius;
        }
        public void SetRadius(Vector2 radius)
        {
            _light.pointLightInnerRadius = radius.x;
            _light.pointLightOuterRadius = radius.y;

        }
    }
}