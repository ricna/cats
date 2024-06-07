using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unrez
{
    public class ProjectileBase : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        protected Rigidbody2D _rb;

        [Header("Debugs")]
        [SerializeField]
        protected float _speed;
        private DealDamageOnContact _dealDamage;

        protected virtual void Awake()
        {
            if (_rb == null)
            {
                _rb = GetComponent<Rigidbody2D>();
            }
            _dealDamage = GetComponent<DealDamageOnContact>();
        }

        public void Fire()
        {
            _rb.AddForce(transform.up * _speed, ForceMode2D.Impulse);
        }

        public virtual void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void SetOwner(ulong owner)
        {
            _dealDamage.SetOwner(owner);
        }
    }
}