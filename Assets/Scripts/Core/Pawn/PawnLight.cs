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

            _light.name = $"PawnLight [{profile.name}]";
            _light.enabled = true;

            _light.lightType = profile.View.LightType;
            _light.color = profile.View.LightColor;
            _light.pointLightInnerRadius = profile.View.LightRadius.x;
            _light.pointLightOuterRadius = profile.View.LightRadius.y;
            _light.intensity = profile.View.LightIntensity;
            _light.falloffIntensity = profile.View.LightFalloffStrenght;
            _light.shadowsEnabled = profile.View.Shadows;
            _light.shadowIntensity = profile.View.ShadowsStrenght;
            _light.shadowSoftness = profile.View.ShadowsSoftness;
            _light.shadowSoftnessFalloffIntensity = profile.View.ShadowsFalloffStrenght;
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