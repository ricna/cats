using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.BackyardShowdown
{
    [RequireComponent(typeof(CompositeShadowCaster2D))]
    public class Structure : MonoBehaviour
    {
        [SerializeField]
        private Color _color = Color.white;
        [SerializeField]
        private SortingLayer _layersToCast;

        private ShadowCaster2D[] _shadowCasters;
        [SerializeField]
        private StructureData _structureData;

        private void Start()
        {
            //_lasyerToCast = LayerMask.NameToLayer("")
            _shadowCasters = GetComponentsInChildren<ShadowCaster2D>();
            foreach (ShadowCaster2D caster2D in _shadowCasters)
            {
                //caster2D.sortingLayer; ;
            }
            UpdateColor();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"OnCollisionEnter2D: {collision.gameObject.name}");
            if (TryGetComponent(out Cat cat))
            {
                _color = cat.GetColor();
                UpdateColor();
            }
            else
            {
                _color = new Color(0.5f, 0.5f, 0.5f);
                UpdateColor();
            }
        }


        public void ChangeColor(Color c)
        {
            _color = c;
            UpdateColor();
        }
        private void UpdateColor()
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = _color;
            }
        }
    }
}