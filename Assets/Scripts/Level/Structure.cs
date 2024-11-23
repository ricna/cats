using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Unrez.BackyardShowdown
{
    [RequireComponent(typeof(CompositeShadowCaster2D))]
    public class Structure : MonoBehaviour
    {
        [SerializeField]
        private StructureData _structureData;

        [Header("Debug")]
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
                caster2D.sortingLayer; ;
            }
            */

            _pieces = GetComponentsInChildren<StructurePiece>();
            foreach (StructurePiece piece in _pieces)
            {
                switch (piece.GetPieceId())
                {
                    case StructurePieceId.CornerBottom:
                        piece.UpdateSprite(_structureData.CornerBottom);
                        break;
                    case StructurePieceId.CornerTop:
                        piece.UpdateSprite(_structureData.CornerTop);
                        break;
                    case StructurePieceId.EndBottom:
                        piece.UpdateSprite(_structureData.EndBottom);
                        break;
                    case StructurePieceId.EndCenter:
                        piece.UpdateSprite(_structureData.EndCenter);
                        break;
                    case StructurePieceId.EndTop:
                        piece.UpdateSprite(_structureData.EndTop);
                        break;
                    case StructurePieceId.RepeatHorizontal:
                        piece.UpdateSprite(_structureData.RepeatHorizontal);
                        break;
                    case StructurePieceId.RepeatVertical:
                        piece.UpdateSprite(_structureData.RepeatVertical);
                        break;
                    case StructurePieceId.TBottom:
                        piece.UpdateSprite(_structureData.TBottom);
                        break;
                    case StructurePieceId.TCenter:
                        piece.UpdateSprite(_structureData.TCenter);
                        break;
                    case StructurePieceId.TTop:
                        piece.UpdateSprite(_structureData.TTop);
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