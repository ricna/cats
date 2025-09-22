using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.BackyardShowdown
{
    public class PawnLight : MonoBehaviour
    {
        [SerializeField]
        protected Light2D _light;
        [Header("Debug")]
        [SerializeField]
        protected Pawn _pawn;
        [SerializeField]
        protected bool _isPrey = false;

        public void SetUp(Pawn pet, PawnProfile profile, Vector2 _colliderOffset)
        {
            _pawn = pet;
            _isPrey = _pawn is Prey;
            if (!_isPrey)
            {
                _light.gameObject.transform.SetParent(_pawn.GetLightPosition().transform);
                _light.gameObject.transform.localPosition = Vector2.zero;
            }
            else
            {
                _light.gameObject.transform.SetParent(_pawn.transform);
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