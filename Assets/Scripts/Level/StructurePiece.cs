using System;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public enum StructurePieceId
    {
        CornerBottom = 0,
        CornerTop = 1,
        EndBottom = 2,
        EndCenter = 3,
        EndTop = 4,
        RepeatHorizontal = 5,
        RepeatVertical = 6,
        TBottom = 7,
        TCenter = 8,
        TTop = 9,
    }

    public class StructurePiece : MonoBehaviour
    {
        private Structure _structure;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private StructurePieceId _pieceId;

        private void Awake()
        {
            _structure = GetComponentInParent<Structure>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /*
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log($"OnCollisionEnter2D: {collision.gameObject.name}");
            if (TryGetComponent(out Cat cat))
            {
                _structure.ChangeColor(cat.GetColor());
            }
            else
            {
                _structure.ChangeColor(new Color(0.5f, 0.5f, 0.5f));
            }
        }*/

        public void UpdateSprite(Sprite sprite)
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            _spriteRenderer.sprite = sprite;
        }

        public StructurePieceId GetPieceId()
        {
            return _pieceId;
        }
    }
}