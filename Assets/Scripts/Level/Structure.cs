using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.BackyardShowdown
{
    [RequireComponent(typeof(CompositeShadowCaster2D))]
    public class Structure : MonoBehaviour
    {
        [SerializeField]
        private StructureData _structureData;

        [SerializeField]
        private StructurePiece[] _pieces;

        private Color _color;
        /*
        private ShadowCaster2D[] _shadowCasters;
        [SerializeField]
        private SortingLayer _layersToCast;
        */

        private void Start()
        {
            /*
            //_lasyerToCast = LayerMask.NameToLayer("")
            _shadowCasters = GetComponentsInChildren<ShadowCaster2D>();
            foreach (ShadowCaster2D caster2D in _shadowCasters)
            {
                //caster2D.sortingLayer; ;
            }
            */

            _pieces = GetComponentsInChildren<StructurePiece>();
            foreach (StructurePiece piece in _pieces)
            {
                switch (piece.GetPieceId())
                {
                    case StructurePieceId.CornerBottom  :
                        piece.UpdateSprite(_structureData.CornerBottom);
                        break;
                }
            }

            _color = _structureData.Color;
            UpdateColor();
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